using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace USPSystem.Controllers;

[Authorize(Roles = "Manager")]
public class VerificationController : BaseController
{
    private readonly ApplicationDbContext _context;

    public VerificationController(
        ApplicationDbContext context, 
        StudentHoldService studentHoldService,
        PageHoldService pageHoldService,
        UserManager<ApplicationUser> userManager) 
        : base(studentHoldService, pageHoldService, userManager)
    {
        _context = context;
    }

    public async Task<IActionResult> VerifyCourses()
    {
        var courses = await _context.Courses
            .Include(c => c.SubjectArea)
            .OrderBy(c => c.Code)
            .ToListAsync();

        return View(courses);
    }
} 

