using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.ApiController;

/// <summary>
/// API controller for manager-specific operations
/// </summary>
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

    /// <summary>
    /// Retrieves all student enrollments in the system
    /// </summary>
    /// <returns>List of all student enrollments with student and course details</returns>
    /// <response code="200">Returns the list of enrollments</response>
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

    /// <summary>
    /// Retrieves detailed information about a specific student
    /// </summary>
    /// <param name="id">The student's ID</param>
    /// <returns>Student details including enrollments</returns>
    /// <response code="200">Returns the student details</response>
    /// <response code="404">If the student is not found</response>
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

    /// <summary>
    /// Updates a student's grade for a specific enrollment
    /// </summary>
    /// <param name="enrollmentId">The enrollment ID</param>
    /// <param name="grade">The new grade to assign</param>
    /// <returns>Success message if grade is updated</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="400">If the grade is invalid</response>
    /// <response code="404">If the enrollment is not found</response>
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
