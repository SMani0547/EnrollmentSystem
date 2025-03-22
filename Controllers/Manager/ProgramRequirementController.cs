using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;

namespace USPEducation.Controllers.Manager;

[Authorize(Roles = "Manager")]
public class ProgramRequirementController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProgramRequirementController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int programId)
    {
        var requirements = await _context.ProgramRequirements
            .Where(r => r.ProgramId == programId)
            .OrderBy(r => r.Year)
            .ToListAsync();

        ViewBag.ProgramId = programId;
        return View(requirements);
    }

    public IActionResult Create(int programId)
    {
        ViewBag.ProgramId = programId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProgramRequirement requirement)
    {
        if (ModelState.IsValid)
        {
            _context.ProgramRequirements.Add(requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { programId = requirement.ProgramId });
        }

        ViewBag.ProgramId = requirement.ProgramId;
        return View(requirement);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement == null)
            return NotFound();

        ViewBag.ProgramId = requirement.ProgramId;
        return View(requirement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProgramRequirement requirement)
    {
        if (id != requirement.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(requirement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { programId = requirement.ProgramId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequirementExists(requirement.Id))
                    return NotFound();
                throw;
            }
        }

        ViewBag.ProgramId = requirement.ProgramId;
        return View(requirement);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement == null)
            return NotFound();

        ViewBag.ProgramId = requirement.ProgramId;
        return View(requirement);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement != null)
        {
            var programId = requirement.ProgramId;
            _context.ProgramRequirements.Remove(requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { programId });
        }

        return NotFound();
    }

    private bool RequirementExists(int id)
    {
        return _context.ProgramRequirements.Any(r => r.Id == id);
    }
} 