using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.Controllers;

[Authorize(Roles = "Manager")]
public class ManagerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStudentGradeService _gradeService;
    private readonly GradeSystemDbContext _gradeSystemDbContext;
    private readonly IEmailService _emailService;

    public ManagerController(
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager,
        IStudentGradeService gradeService,
        GradeSystemDbContext gradeSystemDbContext,
        IEmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _gradeService = gradeService;
        _gradeSystemDbContext = gradeSystemDbContext;
        _emailService = emailService;
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
    
    public async Task<IActionResult> RecheckRequests()
    {
        var requests = await _gradeService.GetAllRecheckRequestsAsync();
        return View(requests);
    }
    
    public async Task<IActionResult> RecheckDetails(int id)
    {
        var requests = await _gradeService.GetAllRecheckRequestsAsync();
        var request = requests.FirstOrDefault(r => r.Id == id);
        
        if (request == null)
            return NotFound();
            
        return View(request);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRecheckStatus(int id, string status)
    {
        try
        {
            // Get the recheck application directly from the database
            var recheckApplication = await _gradeSystemDbContext.RecheckApplications.FindAsync(id);
            
            if (recheckApplication == null)
            {
                TempData["ErrorMessage"] = "Recheck request not found";
                return RedirectToAction(nameof(RecheckRequests));
            }
            
            // Parse the status to enum
            if (Enum.TryParse<GradeRecheckStatus>(status, out var statusEnum))
            {
                // Update the status
                recheckApplication.Status = (int)statusEnum;
                
                // Save changes
                await _gradeSystemDbContext.SaveChangesAsync();
                
                // Send email notification to the student
                await SendStatusUpdateEmailAsync(recheckApplication, status);
                
                TempData["SuccessMessage"] = $"Recheck request status updated to {status}";
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid status value";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Failed to update recheck request status: {ex.Message}";
        }
        
        return RedirectToAction(nameof(RecheckRequests));
    }
    
    private async Task SendStatusUpdateEmailAsync(GradeRecheckApplication recheckApplication, string status)
    {
        try
        {
            // Get student details
            var student = await _context.Users.FirstOrDefaultAsync(u => u.StudentId == recheckApplication.StudentId);
            if (student != null)
            {
                string studentName = $"{student.FirstName} {student.LastName}";
                string studentEmail = recheckApplication.Email;
                
                if (!string.IsNullOrEmpty(studentEmail))
                {
                    // Send email notification
                    bool emailSent = await _emailService.SendRecheckStatusUpdateEmailAsync(
                        studentEmail,
                        studentName,
                        recheckApplication.CourseCode,
                        recheckApplication.CourseName,
                        status
                    );
                    
                    if (emailSent)
                    {
                        Console.WriteLine($"Status update email sent to {studentEmail} for recheck request {recheckApplication.Id}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send status update email to {studentEmail} for recheck request {recheckApplication.Id}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email notification: {ex.Message}");
            // Continue execution, don't throw the exception
        }
    }
} 

