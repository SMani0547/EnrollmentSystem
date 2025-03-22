using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;

namespace USPEducation.Controllers.Manager;

[Authorize(Roles = "Manager")]
public class ProgramController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProgramController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var programs = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.CoreCourses)
            .Include(p => p.ElectiveCourses)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(programs);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AcademicProgram program)
    {
        if (ModelState.IsValid)
        {
            _context.Programs.Add(program);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(program);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var program = await _context.Programs
            .Include(p => p.Requirements)
            .Include(p => p.CoreCourses)
            .Include(p => p.ElectiveCourses)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program == null)
            return NotFound();

        return View(program);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AcademicProgram program)
    {
        if (id != program.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(program);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramExists(program.Id))
                    return NotFound();
                throw;
            }
        }

        return View(program);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var program = await _context.Programs.FindAsync(id);
        if (program == null)
            return NotFound();

        return View(program);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var program = await _context.Programs.FindAsync(id);
        if (program != null)
        {
            _context.Programs.Remove(program);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProgramExists(int id)
    {
        return _context.Programs.Any(p => p.Id == id);
    }
} 