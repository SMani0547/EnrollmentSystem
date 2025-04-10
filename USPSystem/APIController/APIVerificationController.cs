using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.ApiController;

/// <summary>
/// API controller for course verification operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VerificationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VerificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all courses for verification purposes
    /// </summary>
    /// <returns>List of all courses with their subject areas</returns>
    /// <response code="200">Returns the list of courses</response>
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
