using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.Controllers;

[Authorize(Roles = "Manager")]
public class ManagerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ManagerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var enrollments = await _context.StudentEnrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .OrderByDescending(e => e.Year)
            .ThenByDescending(e => e.Semester)
            .ToListAsync();

        return View(enrollments);
    }

    public async Task<IActionResult> StudentDetails(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    public async Task<IActionResult> UpdateGrade(int enrollmentId, string grade)
    {
        var enrollment = await _context.StudentEnrollments
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null)
            return NotFound();

        enrollment.Grade = grade;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(StudentDetails), new { id = enrollment.StudentId });
    }
} 

