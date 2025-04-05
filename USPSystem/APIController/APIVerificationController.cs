using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.ApiController;

[ApiController]
[Route("api/[controller]")]
public class VerificationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VerificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/verification/verifycourses
    [HttpGet("verifycourses")]
    public async Task<ActionResult<IEnumerable<Course>>> VerifyCourses()
    {
        var courses = await _context.Courses
            .Include(c => c.SubjectArea)
            .OrderBy(c => c.Code)
            .ToListAsync();

        return Ok(courses);
    }
}
