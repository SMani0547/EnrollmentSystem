using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.ApiController.Manager;

/// <summary>
/// API controller for managing academic programs
/// </summary>
[Authorize(Roles = "Manager")]
[ApiController]
[Route("api/[controller]")]
public class ProgramController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProgramController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all academic programs
    /// </summary>
    /// <returns>List of all academic programs with their requirements and courses</returns>
    /// <response code="200">Returns the list of programs</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AcademicProgram>>> GetPrograms()
    {
        var programs = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.CoreCourses)
            .Include(p => p.ElectiveCourses)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return Ok(programs);
    }

    /// <summary>
    /// Retrieves a specific academic program by ID
    /// </summary>
    /// <param name="id">The program ID</param>
    /// <returns>The academic program details</returns>
    /// <response code="200">Returns the program details</response>
    /// <response code="404">If the program is not found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<AcademicProgram>> GetProgram(int id)
    {
        var program = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.CoreCourses)
            .Include(p => p.ElectiveCourses)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
            return NotFound();

        return Ok(program);
    }

    /// <summary>
    /// Creates a new academic program
    /// </summary>
    /// <param name="program">The program details to create</param>
    /// <returns>The created program with its ID</returns>
    /// <response code="201">Returns the created program</response>
    /// <response code="400">If the program data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<AcademicProgram>> CreateProgram(AcademicProgram program)
    {
        _context.Programs.Add(program);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
    }

    /// <summary>
    /// Updates an existing academic program
    /// </summary>
    /// <param name="id">The program ID</param>
    /// <param name="program">The updated program details</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the update is successful</response>
    /// <response code="400">If the program data is invalid</response>
    /// <response code="404">If the program is not found</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgram(int id, AcademicProgram program)
    {
        if (id != program.Id)
            return BadRequest();

        _context.Entry(program).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProgramExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/program/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgram(int id)
    {
        var program = await _context.Programs.FindAsync(id);
        if (program == null)
            return NotFound();

        _context.Programs.Remove(program);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProgramExists(int id)
    {
        return _context.Programs.Any(p => p.Id == id);
    }
}
