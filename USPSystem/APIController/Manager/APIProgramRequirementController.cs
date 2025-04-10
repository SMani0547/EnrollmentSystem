using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.ApiController.Manager;

/// <summary>
/// API controller for managing program requirements
/// </summary>
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

    /// <summary>
    /// Retrieves all program requirements
    /// </summary>
    /// <returns>List of all program requirements</returns>
    /// <response code="200">Returns the list of requirements</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProgramRequirement>>> GetRequirements()
    {
        var requirements = await _context.ProgramRequirements
            .OrderBy(r => r.Year)
            .ToListAsync();

        return Ok(requirements);
    }

    /// <summary>
    /// Retrieves a specific program requirement by ID
    /// </summary>
    /// <param name="id">The requirement ID</param>
    /// <returns>The program requirement details</returns>
    /// <response code="200">Returns the requirement details</response>
    /// <response code="404">If the requirement is not found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProgramRequirement>> GetRequirement(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement == null)
            return NotFound();

        return Ok(requirement);
    }

    /// <summary>
    /// Creates a new program requirement
    /// </summary>
    /// <param name="requirement">The requirement details to create</param>
    /// <returns>The created requirement with its ID</returns>
    /// <response code="201">Returns the created requirement</response>
    /// <response code="400">If the requirement data is invalid</response>
    [HttpPost]
    public async Task<ActionResult<ProgramRequirement>> CreateRequirement(ProgramRequirement requirement)
    {
        _context.ProgramRequirements.Add(requirement);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRequirement), new { id = requirement.Id }, requirement);
    }

    /// <summary>
    /// Updates an existing program requirement
    /// </summary>
    /// <param name="id">The requirement ID</param>
    /// <param name="requirement">The updated requirement details</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the update is successful</response>
    /// <response code="400">If the requirement data is invalid</response>
    /// <response code="404">If the requirement is not found</response>
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
