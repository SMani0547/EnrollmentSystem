using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.ApiController.Manager;

[Authorize(Roles = "Manager")]
[ApiController]
[Route("api/[controller]")]
public class ProgramRequirementController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProgramRequirementController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/programrequirement/program/5
    [HttpGet("program/{programId}")]
    public async Task<ActionResult<IEnumerable<ProgramRequirement>>> GetRequirementsForProgram(int programId)
    {
        var requirements = await _context.ProgramRequirements
            .Where(r => r.ProgramId == programId)
            .OrderBy(r => r.Year)
            .ToListAsync();

        return Ok(requirements);
    }

    // GET: api/programrequirement/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProgramRequirement>> GetRequirement(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement == null)
            return NotFound();

        return Ok(requirement);
    }

    // POST: api/programrequirement
    [HttpPost]
    public async Task<ActionResult<ProgramRequirement>> CreateRequirement(ProgramRequirement requirement)
    {
        _context.ProgramRequirements.Add(requirement);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRequirement), new { id = requirement.Id }, requirement);
    }

    // PUT: api/programrequirement/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRequirement(int id, ProgramRequirement requirement)
    {
        if (id != requirement.Id)
            return BadRequest();

        _context.Entry(requirement).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RequirementExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/programrequirement/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequirement(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement == null)
            return NotFound();

        _context.ProgramRequirements.Remove(requirement);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RequirementExists(int id)
    {
        return _context.ProgramRequirements.Any(r => r.Id == id);
    }
}
