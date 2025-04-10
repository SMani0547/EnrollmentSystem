using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.APIController;

/// <summary>
/// API controller for student-related operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class APIStudentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStudentGradeService _gradeService;

    public APIStudentController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IStudentGradeService gradeService)
    {
        _context = context;
        _userManager = userManager;
        _gradeService = gradeService;
    }

    /// <summary>
    /// Gets all enrollments for the current student
    /// </summary>
    /// <returns>A list of student enrollments with course details</returns>
    /// <response code="200">Returns the list of enrollments</response>
    /// <response code="404">If the student is not found</response>
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

    /// <summary>
    /// Gets program requirements for the current student
    /// </summary>
    /// <returns>Program requirements including major, minor, and completed courses</returns>
    /// <response code="200">Returns the program requirements</response>
    /// <response code="404">If the student is not found</response>
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

    /// <summary>
    /// Gets available courses for enrollment
    /// </summary>
    /// <returns>A list of courses that the student can enroll in</returns>
    /// <response code="200">Returns the list of available courses</response>
    /// <response code="404">If the student is not found</response>
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

    /// <summary>
    /// Gets detailed information about a specific course
    /// </summary>
    /// <param name="id">The course ID</param>
    /// <returns>Course details including prerequisites</returns>
    /// <response code="200">Returns the course details</response>
    /// <response code="404">If the course or student is not found</response>
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

    /// <summary>
    /// Enrolls the student in a course
    /// </summary>
    /// <param name="courseId">The ID of the course to enroll in</param>
    /// <returns>Success message if enrollment is successful</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="400">If already enrolled or prerequisites not met</response>
    /// <response code="404">If the course or student is not found</response>
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

    /// <summary>
    /// Unenrolls the student from a course
    /// </summary>
    /// <param name="enrollmentId">The ID of the enrollment to remove</param>
    /// <returns>Success message if unenrollment is successful</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="404">If the enrollment or student is not found</response>
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

    /// <summary>
    /// Gets the student's profile information
    /// </summary>
    /// <returns>Student profile details including enrollments</returns>
    /// <response code="200">Returns the student profile</response>
    /// <response code="404">If the student is not found</response>
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

    /// <summary>
    /// Gets the student's grades
    /// </summary>
    /// <returns>A list of grades with course information</returns>
    /// <response code="200">Returns the list of grades</response>
    /// <response code="404">If the student is not found</response>
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
