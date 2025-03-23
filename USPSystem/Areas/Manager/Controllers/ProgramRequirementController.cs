using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPEducation.Areas.Manager.Controllers;

[Area("Manager")]
[Authorize(Roles = "Manager")]
public class ProgramRequirementController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProgramRequirementController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var requirements = await _context.ProgramRequirements
            .Include(r => r.SubjectArea)
            .Include(r => r.RequiredCourses)
            .ToListAsync();

        return View(requirements);
    }

    public IActionResult Create()
    {
        LoadViewBagData();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProgramRequirement requirement, int[] selectedCourses)
    {
        if (ModelState.IsValid)
        {
            if (selectedCourses != null && selectedCourses.Any())
            {
                requirement.RequiredCourses = await _context.Courses
                    .Where(c => selectedCourses.Contains(c.Id))
                    .ToListAsync();
            }

            _context.Add(requirement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        LoadViewBagData();
        return View(requirement);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var requirement = await _context.ProgramRequirements
            .Include(r => r.RequiredCourses)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (requirement == null)
        {
            return NotFound();
        }

        LoadViewBagData();
        return View(requirement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProgramRequirement requirement, int[] selectedCourses)
    {
        if (id != requirement.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingRequirement = await _context.ProgramRequirements
                    .Include(r => r.RequiredCourses)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (existingRequirement == null)
                {
                    return NotFound();
                }

                existingRequirement.SubjectAreaId = requirement.SubjectAreaId;
                existingRequirement.Type = requirement.Type;
                existingRequirement.Year = requirement.Year;
                existingRequirement.CreditPointsRequired = requirement.CreditPointsRequired;
                existingRequirement.MinimumGrade = requirement.MinimumGrade;
                existingRequirement.Description = requirement.Description;
                existingRequirement.Notes = requirement.Notes;

                existingRequirement.RequiredCourses.Clear();
                if (selectedCourses != null && selectedCourses.Any())
                {
                    var selectedCoursesList = await _context.Courses
                        .Where(c => selectedCourses.Contains(c.Id))
                        .ToListAsync();
                    foreach (var course in selectedCoursesList)
                    {
                        existingRequirement.RequiredCourses.Add(course);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramRequirementExists(requirement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        LoadViewBagData();
        return View(requirement);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var requirement = await _context.ProgramRequirements
            .Include(r => r.SubjectArea)
            .Include(r => r.RequiredCourses)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (requirement == null)
        {
            return NotFound();
        }

        return View(requirement);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var requirement = await _context.ProgramRequirements.FindAsync(id);
        if (requirement == null)
        {
            return NotFound();
        }

        _context.ProgramRequirements.Remove(requirement);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProgramRequirementExists(int id)
    {
        return _context.ProgramRequirements.Any(e => e.Id == id);
    }

    private void LoadViewBagData()
    {
        ViewBag.SubjectAreas = new SelectList(_context.SubjectAreas, "Id", "Name");
        ViewBag.Courses = new SelectList(_context.Courses, "Id", "Name");
    }
} 

