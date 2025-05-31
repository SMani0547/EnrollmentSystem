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

    public ManagerController(
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
        if (await _gradeService.UpdateRecheckStatusAsync(id, status))
        {
            TempData["SuccessMessage"] = $"Recheck request status updated to {status}";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to update recheck request status";
        }
        
        return RedirectToAction(nameof(RecheckRequests));
    }
} 

