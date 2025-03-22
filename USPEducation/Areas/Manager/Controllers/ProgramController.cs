using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;
using USPEducation.Models.ViewModels;

namespace USPEducation.Areas.Manager.Controllers;

[Area("Manager")]
[Authorize(Roles = "Manager")]
public class ProgramController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProgramController> _logger;

    public ProgramController(ApplicationDbContext context, ILogger<ProgramController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var programs = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.CoreCourses)
            .Include(p => p.ElectiveCourses)
            .OrderByDescending(p => p.OfferingYear)
            .ThenBy(p => p.Level)
            .ThenBy(p => p.Name)
            .ToListAsync();

        return View(programs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var program = await _context.Programs
            .Include(p => p.Requirements)
            .ThenInclude(r => r.SubjectArea)
            .Include(p => p.Requirements)
            .ThenInclude(r => r.RequiredCourses)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found", id);
            return NotFound();
        }

        return View(program);
    }

    public async Task<IActionResult> Structure(int id)
    {
        var program = await _context.Programs
            .Include(p => p.Requirements)
            .ThenInclude(r => r.SubjectArea)
            .Include(p => p.Requirements)
            .ThenInclude(r => r.RequiredCourses)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found", id);
            return NotFound();
        }

        var viewModel = new ProgramStructureViewModel
        {
            Id = program.Id,
            Code = program.Code,
            Name = program.Name,
            Description = program.Description,
            CreditPoints = program.CreditPoints,
            Duration = program.Duration,
            Level = program.Level,
            Requirements = program.Requirements,
            ProgramId = program.Id,
            ProgramName = program.Name,
            ProgramCode = program.Code,
            OfferingYear = program.OfferingYear,
            MajorCreditsRequired = program.MajorCreditsRequired,
            MinorCreditsRequired = program.MinorCreditsRequired,
            IsActive = program.IsActive
        };

        // Get available majors and minors
        var subjectAreas = await _context.SubjectAreas
            .Where(sa => sa.IsActive)
            .ToListAsync();

        var majorRequirements = program.Requirements
            .Where(r => r.Type == RequirementType.MajorCore)
            .Select(r => r.SubjectArea)
            .Distinct()
            .ToList();

        var minorRequirements = program.Requirements
            .Where(r => r.Type == RequirementType.MinorCore)
            .Select(r => r.SubjectArea)
            .Distinct()
            .ToList();

        viewModel.AvailableMajors = majorRequirements;
        viewModel.AvailableMinors = minorRequirements;

        return View(viewModel);
    }

    public IActionResult Create()
    {
        var program = new AcademicProgram
        {
            OfferingYear = DateTime.Now.Year + 1,
            IsActive = true,
            MajorCreditsRequired = 48,
            MinorCreditsRequired = 24
        };
        return View(program);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AcademicProgram program)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(program);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created new program: {ProgramCode} - {ProgramName}", program.Code, program.Name);
                TempData["Success"] = "Program created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating program: {ProgramCode}", program.Code);
                ModelState.AddModelError("", "An error occurred while creating the program. Please try again.");
            }
        }
        return View(program);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var program = await _context.Programs.FindAsync(id);
        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found", id);
            return NotFound();
        }
        return View(program);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AcademicProgram program)
    {
        if (id != program.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(program);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated program: {ProgramCode} - {ProgramName}", program.Code, program.Name);
                TempData["Success"] = "Program updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProgramExists(program.Id))
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found during update", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating program: {ProgramCode}", program.Code);
                    ModelState.AddModelError("", "An error occurred while updating the program. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating program: {ProgramCode}", program.Code);
                ModelState.AddModelError("", "An error occurred while updating the program. Please try again.");
            }
        }
        return View(program);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var program = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.StudentEnrollments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found", id);
            return NotFound();
        }

        if (program.StudentEnrollments.Any())
        {
            TempData["Error"] = "Cannot delete program with active student enrollments.";
            return RedirectToAction(nameof(Index));
        }

        return View(program);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var program = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.StudentEnrollments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found during delete", id);
            return NotFound();
        }

        if (program.StudentEnrollments.Any())
        {
            TempData["Error"] = "Cannot delete program with active student enrollments.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted program: {ProgramCode} - {ProgramName}", program.Code, program.Name);
            TempData["Success"] = "Program deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting program: {ProgramCode}", program.Code);
            TempData["Error"] = "An error occurred while deleting the program.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Structure(ProgramStructureViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var program = await _context.Programs
            .Include(p => p.Requirements)
            .FirstOrDefaultAsync(p => p.Id == viewModel.ProgramId);

        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found during structure update", viewModel.ProgramId);
            return NotFound();
        }

        try
        {
            // Update program credits
            program.MajorCreditsRequired = viewModel.MajorCreditsRequired;
            program.MinorCreditsRequired = viewModel.MinorCreditsRequired;
            program.OfferingYear = viewModel.OfferingYear;
            program.IsActive = viewModel.IsActive;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated program structure: {ProgramCode}", program.Code);
            TempData["Success"] = "Program structure updated successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating program structure: {ProgramCode}", program.Code);
            TempData["Error"] = "An error occurred while updating the program structure.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProgramExists(int id)
    {
        return _context.Programs.Any(e => e.Id == id);
    }
} 