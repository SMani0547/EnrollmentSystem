using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.Controllers;

public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStudentGradeService _gradeService;

    public StudentController(
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager,
        IStudentGradeService gradeService)
    {
        _context = context;
        _userManager = userManager;
        _gradeService = gradeService;
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

    public IActionResult Enroll()
    {
        return RedirectToAction(nameof(AvailableCourses));
    }

    public async Task<IActionResult> Requirements()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        // Get completed courses from external grade system
        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);
        ViewBag.CompletedCourses = completedCourseIds;

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

    private bool IsCourseCompleted(int courseId)
    {
        return ViewBag.CompletedCourses?.Contains(courseId) ?? false;
    }

    public async Task<IActionResult> AvailableCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        // Get completed courses from external grade system
        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);

        ViewBag.CompletedCourses = completedCourseIds;

        var enrolledCourseIds = await _context.StudentEnrollments
            .Where(e => e.StudentId == user.Id)
            .Select(e => e.CourseId)
            .ToListAsync();

        var availableCourses = await _context.Courses
            .Include(c => c.Prerequisites)
            .Where(c => !enrolledCourseIds.Contains(c.Id))
            .Where(p => !completedCourseIds.Contains(p.Id))
            .OrderBy(c => c.Code)
            .ToListAsync();

        return View(availableCourses);
    }

    public async Task<IActionResult> EnrollDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var course = await _context.Courses
            .Include(c => c.Prerequisites)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            return NotFound();

        // Get completed courses from external grade system
        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);

        ViewBag.CompletedCourses = completedCourseIds;

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

        // Check prerequisites using external grade system
        if (course.Prerequisites.Any())
        {
            var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);
            var prerequisiteIds = course.Prerequisites.Select(p => p.Id).ToList();
            var completedPrerequisites = prerequisiteIds.Count(id => completedCourseIds.Contains(id));

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

    public async Task<IActionResult> CourseDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var course = await _context.Courses
            .Include(c => c.Prerequisites)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
            return NotFound();

        // Get completed courses from external grade system
        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);

        ViewBag.CompletedCourses = completedCourseIds;

        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unenroll(int enrollmentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var enrollment = await _context.StudentEnrollments
            .FirstOrDefaultAsync(e => e.Id == enrollmentId && e.StudentId == user.Id);

        if (enrollment == null)
            return NotFound();

        _context.StudentEnrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Successfully unenrolled from the course.";
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

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        // Load the complete user profile with related data
        var profile = await _context.Users
            .Include(u => u.Enrollments)
                .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (profile == null)
            return NotFound();

        return View(profile);
    }
}

