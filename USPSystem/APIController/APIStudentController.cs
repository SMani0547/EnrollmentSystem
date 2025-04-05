using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.APIController;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
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

    [HttpGet("enrollments")]
    public async Task<IActionResult> GetEnrollments()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var enrollments = await _context.StudentEnrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == user.Id)
            .OrderBy(e => e.Year)
            .ThenBy(e => e.Semester)
            .ToListAsync();

        return Ok(enrollments);
    }

    [HttpGet("requirements")]
    public async Task<IActionResult> GetRequirements()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);

        var requirements = await _context.ProgramRequirements
            .Include(r => r.SubjectArea)
            .Include(r => r.RequiredCourses)
            .Where(r => r.Year <= user.AdmissionYear && r.IsActive)
            .ToListAsync();

        var coreRequirements = requirements.Where(r =>
            r.Type == RequirementType.MajorCore ||
            r.Type == RequirementType.MinorCore ||
            r.Type == RequirementType.ProgressionRequirement).ToList();

        var majorRequirements = requirements.Where(r =>
            (r.Type == RequirementType.MajorCore || r.Type == RequirementType.MajorElective) &&
            r.SubjectArea.Code == user.MajorI).ToList();

        if (user.MajorType == MajorType.DoubleMajor)
        {
            majorRequirements.AddRange(requirements.Where(r =>
                (r.Type == RequirementType.MajorCore || r.Type == RequirementType.MajorElective) &&
                r.SubjectArea.Code == user.MajorII));
        }
        else if (user.MajorType == MajorType.MajorMinor)
        {
            majorRequirements.AddRange(requirements.Where(r =>
                (r.Type == RequirementType.MinorCore || r.Type == RequirementType.MinorElective) &&
                r.SubjectArea.Code == user.MinorI));
        }

        return Ok(new
        {
            MajorType = user.MajorType,
            MajorI = user.MajorI,
            MajorII = user.MajorII,
            MinorI = user.MinorI,
            AdmissionYear = user.AdmissionYear,
            CompletedCourses = completedCourseIds,
            Requirements = majorRequirements
        });
    }

    [HttpGet("available-courses")]
    public async Task<IActionResult> GetAvailableCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);

        var enrolledCourseIds = await _context.StudentEnrollments
            .Where(e => e.StudentId == user.Id)
            .Select(e => e.CourseId)
            .ToListAsync();

        var availableCourses = await _context.Courses
            .Include(c => c.Prerequisites)
            .Where(c => !enrolledCourseIds.Contains(c.Id) && !completedCourseIds.Contains(c.Id))
            .OrderBy(c => c.Code)
            .ToListAsync();

        return Ok(availableCourses);
    }

    [HttpGet("course/{id}")]
    public async Task<IActionResult> GetCourseDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var course = await _context.Courses
            .Include(c => c.Prerequisites)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null) return NotFound();

        var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);

        return Ok(new { course, CompletedCourses = completedCourseIds });
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> EnrollCourse([FromBody] int courseId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var course = await _context.Courses
            .Include(c => c.Prerequisites)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return NotFound();

        var alreadyEnrolled = await _context.StudentEnrollments
            .AnyAsync(e => e.StudentId == user.Id && e.CourseId == courseId);

        if (alreadyEnrolled)
            return BadRequest("Already enrolled in this course.");

        if (course.Prerequisites.Any())
        {
            var completedCourseIds = await _gradeService.GetCompletedCourseIdsAsync(user.StudentId);
            var prerequisiteIds = course.Prerequisites.Select(p => p.Id).ToList();

            var completedCount = prerequisiteIds.Count(id => completedCourseIds.Contains(id));
            if (completedCount < prerequisiteIds.Count)
                return BadRequest("Prerequisites not met.");
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

        return Ok("Successfully enrolled.");
    }

    [HttpDelete("unenroll/{enrollmentId}")]
    public async Task<IActionResult> Unenroll(int enrollmentId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var enrollment = await _context.StudentEnrollments
            .FirstOrDefaultAsync(e => e.Id == enrollmentId && e.StudentId == user.Id);

        if (enrollment == null) return NotFound();

        _context.StudentEnrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Successfully unenrolled.");
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var profile = await _context.Users
            .Include(u => u.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (profile == null) return NotFound();

        return Ok(profile);
    }

    [HttpGet("grades")]
    [Authorize]
    public async Task<IActionResult> GetGrades()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var grades = await _gradeService.GetGradesAsync(user.StudentId) ?? new List<GradeViewModel>();

        foreach (var grade in grades)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Code == grade.CourseCode);
            if (course != null)
            {
                grade.CourseName = course.Name;
            }
        }

        return Ok(grades);
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
