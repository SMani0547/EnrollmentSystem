using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;

namespace USPEducation.Controllers;

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
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .OrderByDescending(e => e.Year)
            .ThenByDescending(e => e.Semester)
            .ToListAsync();

        return View(enrollments);
    }

    public async Task<IActionResult> StudentDetails(string id)
    {
        var student = await _userManager.FindByIdAsync(id);
        if (student == null)
            return NotFound();

        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == id)
            .OrderByDescending(e => e.Year)
            .ThenByDescending(e => e.Semester)
            .ToListAsync();

        ViewBag.Student = student;
        return View(enrollments);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateGrade(int enrollmentId, string grade)
    {
        var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
        if (enrollment == null)
            return NotFound();

        enrollment.Grade = grade;
        await _context.SaveChangesAsync();

        TempData["Success"] = "Grade updated successfully.";
        return RedirectToAction(nameof(StudentDetails), new { id = enrollment.StudentId });
    }

    public async Task<IActionResult> Students()
    {
        var students = await _userManager.GetUsersInRoleAsync("Student");
        return View(students);
    }

    public async Task<IActionResult> Courses()
    {
        var courses = await _context.Courses.ToListAsync();
        return View(courses);
    }
} 