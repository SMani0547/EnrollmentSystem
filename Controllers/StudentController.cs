using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;

namespace USPEducation.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == user.Id)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync();

        return View(enrollments);
    }

    public async Task<IActionResult> AvailableCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        var enrolledCourseIds = await _context.Enrollments
            .Where(e => e.StudentId == user.Id)
            .Select(e => e.CourseId)
            .ToListAsync();

        var availableCourses = await _context.Courses
            .Where(c => !enrolledCourseIds.Contains(c.Id))
            .ToListAsync();

        return View(availableCourses);
    }

    [HttpPost]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var user = await _userManager.GetUserAsync(User);
        var course = await _context.Courses.FindAsync(courseId);

        if (course == null)
        {
            return NotFound();
        }

        var enrollment = new Enrollment
        {
            StudentId = user.Id,
            CourseId = courseId,
            EnrollmentDate = DateTime.UtcNow,
            Status = EnrollmentStatus.Pending
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
} 