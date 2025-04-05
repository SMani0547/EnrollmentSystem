using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.ApiController;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class ManagerController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ManagerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: api/manager/enrollments
    [HttpGet("enrollments")]
    public async Task<IActionResult> GetAllEnrollments()
    {
        var enrollments = await _context.StudentEnrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .OrderByDescending(e => e.Year)
            .ThenByDescending(e => e.Semester)
            .ToListAsync();

        return Ok(enrollments);
    }

    // GET: api/manager/student/{id}
    [HttpGet("student/{id}")]
    public async Task<IActionResult> GetStudentDetails(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound(new { message = "Student not found" });

        return Ok(user);
    }

    // PUT: api/manager/update-grade?enrollmentId=5&grade=B+
    [HttpPut("updategrade")]
    public async Task<IActionResult> UpdateGrade(int enrollmentId, string grade)
    {
        var enrollment = await _context.StudentEnrollments
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null)
            return NotFound(new { message = "Enrollment not found" });

        enrollment.Grade = grade;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Grade updated successfully", enrollment });
    }
}
