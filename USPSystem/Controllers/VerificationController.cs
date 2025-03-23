using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.Controllers;

public class VerificationController : Controller
{
    private readonly ApplicationDbContext _context;

    public VerificationController(ApplicationDbContext context)
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

