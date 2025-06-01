using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

    // ✅ TRANSCRIPT FEATURE – START

    [Authorize]
    [HttpGet]
    public IActionResult Transcript()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateTranscript()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        string studentId = user.StudentId;

        var client = new HttpClient();
        var authBytes = Encoding.ASCII.GetBytes("admin:password123"); // ← use this
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

        var response = await client.GetAsync($"http://localhost:5240/transcript/{studentId}");

        if (response.IsSuccessStatusCode)
        {
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return File(fileBytes, "application/pdf", $"Transcript_{studentId}.pdf");
        }
        else
        {
            // ✅ Read the actual response content from Flask
            string errorDetails = await response.Content.ReadAsStringAsync();

            // ✅ Optional: Log it or print to console
            Console.WriteLine($"Transcript API Error: {errorDetails}");

            // ✅ Show error on page
            TempData["Error"] = $"Unable to retrieve transcript. API returned: {errorDetails}";
            return RedirectToAction("Transcript");
        }


    }

    // ✅ TRANSCRIPT FEATURE – END

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

        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);
        ViewBag.CompletedCourses = completedCourseIds;

        var requirements = await _context.ProgramRequirements
            .Include(r => r.SubjectArea)
            .Include(r => r.RequiredCourses)
            .Where(r => r.Year <= user.AdmissionYear && r.IsActive)
            .ToListAsync();

        var coreRequirements = requirements.Where(r =>
            r.Type == RequirementType.MajorCore ||
            r.Type == RequirementType.MinorCore ||
            r.Type == RequirementType.ProgressionRequirement
        ).ToList();

        var majorRequirements = requirements.Where(r =>
            (r.Type == RequirementType.MajorCore || r.Type == RequirementType.MajorElective) &&
            r.SubjectArea.Code == user.MajorI
        ).ToList();

        if (user.MajorType == MajorType.DoubleMajor)
        {
            majorRequirements.AddRange(requirements.Where(r =>
                (r.Type == RequirementType.MajorCore || r.Type == RequirementType.MajorElective) &&
                r.SubjectArea.Code == user.MajorII
            ));
        }
        else if (user.MajorType == MajorType.MajorMinor)
        {
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

        var existingEnrollment = await _context.StudentEnrollments
            .AnyAsync(e => e.StudentId == user.Id && e.CourseId == courseId);

        if (existingEnrollment)
        {
            ModelState.AddModelError("", "You are already enrolled in this course.");
            return View(course);
        }

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

        var profile = await _context.Users
            .Include(u => u.Enrollments)
                .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (profile == null)
            return NotFound();

        return View(profile);
    }

    [Authorize]
    public async Task<IActionResult> Grades()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var grades = await _gradeService.GetGradesAsync(user.StudentId);
        if (grades == null)
            grades = new List<GradeViewModel>();

        foreach (var grade in grades)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Code == grade.CourseCode);
            if (course != null)
            {
                grade.CourseName = course.Name;
            }

            try
            {
                grade.HasAppliedForRecheck = await _gradeService.HasAppliedForRecheckAsync(user.StudentId, grade.CourseCode, grade.Year, grade.Semester);
            }
            catch
            {
                grade.HasAppliedForRecheck = false;
            }
        }

        return View(grades);
    }

    [Authorize]
    public async Task<IActionResult> ApplyForRecheck(int gradeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var grades = await _gradeService.GetGradesAsync(user.StudentId);
        var grade = grades?.FirstOrDefault(g => g.Id == gradeId);
        if (grade == null)
            return NotFound();

        if (await _gradeService.HasAppliedForRecheckAsync(user.StudentId, grade.CourseCode, grade.Year, grade.Semester))
        {
            TempData["ErrorMessage"] = "You have already applied for a recheck for this course.";
            return RedirectToAction(nameof(Grades));
        }

        var model = new RecheckApplicationModel
        {
            GradeId = grade.Id,
            CourseCode = grade.CourseCode,
            CourseName = grade.CourseName,
            Year = grade.Year,
            Semester = grade.Semester,
            CurrentGrade = grade.Grade,
            Email = user.Email ?? ""
        };

        return View("RecheckApplication", model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitRecheckApplication(RecheckApplicationModel model)
    {
        if (!ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var grades = await _gradeService.GetGradesAsync(user.StudentId);
            var grade = grades?.FirstOrDefault(g => g.Id == model.GradeId);

            if (grade != null)
            {
                model.CourseCode = grade.CourseCode;
                model.CourseName = grade.CourseName;
                model.CurrentGrade = grade.Grade;
                model.Year = model.Year > 0 ? model.Year : grade.Year;
                model.Semester = model.Semester > 0 ? model.Semester : grade.Semester;
            }

            return View("RecheckApplication", model);
        }

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound();

        try
        {
            var result = await _gradeService.ApplyForRecheckAsync(
                currentUser.StudentId,
                model.CourseCode,
                model.Year,
                model.Semester,
                model.Reason,
                model.AdditionalComments,
                model.Email,
                model.CourseName,
                model.CurrentGrade,
                model.PaymentReceiptNumber);

            if (result)
            {
                TempData["SuccessMessage"] = "Your recheck application has been submitted successfully.";
                return RedirectToAction(nameof(Grades));
            }

            TempData["ErrorMessage"] = "Failed to submit recheck application.";
            return RedirectToAction(nameof(Grades));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            return View("RecheckApplication", model);
        }
    }
}
