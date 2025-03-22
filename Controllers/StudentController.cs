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

        var enrollments = await _context.StudentEnrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == user.Id)
            .OrderBy(e => e.Year)
            .ThenBy(e => e.Semester)
            .ToListAsync();

        return View(enrollments);
    }

    public async Task<IActionResult> Requirements()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var requirements = await _context.ProgramRequirements
            .Include(r => r.SubjectArea)
            .Include(r => r.RequiredCourses)
            .Where(r => r.Year <= user.AdmissionYear && r.IsActive)
            .ToListAsync();

        // Get core requirements
        var coreRequirements = requirements.Where(r => 
            r.Type == RequirementType.MajorCore ||
            r.Type == RequirementType.MinorCore ||
            r.Type == RequirementType.ProgressionRequirement
        ).ToList();

        // Get major requirements
        var majorRequirements = requirements.Where(r => 
            (r.Type == RequirementType.MajorCore || r.Type == RequirementType.MajorElective) &&
            r.SubjectArea.Code == user.MajorI
        ).ToList();

        if (user.MajorType == MajorType.DoubleMajor)
        {
            // Add second major requirements
            majorRequirements.AddRange(requirements.Where(r => 
                (r.Type == RequirementType.MajorCore || r.Type == RequirementType.MajorElective) &&
                r.SubjectArea.Code == user.MajorII
            ));
        }
        else if (user.MajorType == MajorType.MajorMinor)
        {
            // Add minor requirements
            majorRequirements.AddRange(requirements.Where(r => 
                (r.Type == RequirementType.MinorCore || r.Type == RequirementType.MinorElective) &&
                r.SubjectArea.Code == user.MinorI
            ));
        }

        ViewBag.MajorType = user.MajorType;
        ViewBag.MajorI = user.MajorI;
        ViewBag.MajorII = user.MajorII;
        ViewBag.MinorI = user.MinorI;
        ViewBag.AdmissionYear = user.AdmissionYear;

        return View(majorRequirements);
    }

    public async Task<IActionResult> AvailableCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var enrolledCourseIds = await _context.StudentEnrollments
            .Where(e => e.StudentId == user.Id)
            .Select(e => e.CourseId)
            .ToListAsync();

        var availableCourses = await _context.Courses
            .Where(c => !enrolledCourseIds.Contains(c.Id))
            .OrderBy(c => c.Code)
            .ToListAsync();

        return View(availableCourses);
    }

    public async Task<IActionResult> EnrollDetails(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Prerequisites)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            return NotFound();

        return View("Enroll", course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var course = await _context.Courses
            .Include(c => c.Prerequisites)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            return NotFound();

        // Check if already enrolled
        var existingEnrollment = await _context.StudentEnrollments
            .AnyAsync(e => e.StudentId == user.Id && e.CourseId == courseId);

        if (existingEnrollment)
        {
            ModelState.AddModelError("", "You are already enrolled in this course.");
            return View(course);
        }

        // Check prerequisites
        if (course.Prerequisites.Any())
        {
            var prerequisiteIds = course.Prerequisites.Select(p => p.Id).ToList();
            var completedPrerequisites = await _context.StudentEnrollments
                .CountAsync(e => e.StudentId == user.Id && 
                               prerequisiteIds.Contains(e.CourseId) && 
                               e.Grade != null && 
                               e.Grade != "F");

            if (completedPrerequisites < course.Prerequisites.Count)
            {
                ModelState.AddModelError("", "You have not completed all prerequisites for this course.");
                return View(course);
            }
        }

        var enrollment = new StudentEnrollment
        {
            StudentId = user.Id,
            CourseId = courseId,
            Year = DateTime.Now.Year,
            Semester = GetCurrentSemester()
        };

        _context.StudentEnrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private static Semester GetCurrentSemester()
    {
        var month = DateTime.Now.Month;
        return month switch
        {
            >= 1 and <= 6 => Semester.Semester1,
            >= 7 and <= 11 => Semester.Semester2,
            _ => Semester.Semester1
        };
    }
} 