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
        if (user == null)
            return NotFound();

        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == user.Id)
            .OrderByDescending(e => e.Year)
            .ThenByDescending(e => e.Semester)
            .ToListAsync();

        return View(enrollments);
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        // Load related data
        user = await _context.Users
            .Include(u => u.StudentAddress)
            .Include(u => u.EmergencyContact)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    public IActionResult Enroll()
    {
        var courses = _context.Courses.ToList();
        return View(courses);
    }

    [HttpPost]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
            return NotFound();

        // Check if already enrolled
        var existingEnrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == user.Id && e.CourseId == courseId);

        if (existingEnrollment != null)
        {
            TempData["Error"] = "You are already enrolled in this course.";
            return RedirectToAction(nameof(Enroll));
        }

        var enrollment = new Enrollment
        {
            StudentId = user.Id,
            CourseId = courseId,
            Semester = GetCurrentSemester(),
            Year = DateTime.Now.Year
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Successfully enrolled in the course.";
        return RedirectToAction(nameof(Index));
    }

    private string GetCurrentSemester()
    {
        var month = DateTime.Now.Month;
        return month switch
        {
            >= 1 and <= 4 => "Semester 1",
            >= 5 and <= 8 => "Semester 2",
            _ => "Summer"
        };
    }
} 