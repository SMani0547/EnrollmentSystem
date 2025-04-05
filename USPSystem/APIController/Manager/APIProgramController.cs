using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.ApiController.Manager;

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

    // GET: api/program
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

    // GET: api/program/{id}
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

    // POST: api/program
    [HttpPost]
    public async Task<ActionResult<AcademicProgram>> CreateProgram(AcademicProgram program)
    {
        _context.Programs.Add(program);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
    }

    // PUT: api/program/{id}
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
