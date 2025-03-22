using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USPEducation.Data;
using USPEducation.Models;

namespace USPEducation.Controllers;

[Authorize(Roles = "Manager")]
public class AdminFinanceController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminFinanceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Existing actions

    // New Action for Admin Finance Menu
    public async Task<IActionResult> FinanceMenu()
{
    var studentFinance = await _context.StudentFinances
        .OrderBy(sf => sf.StudentID)
        .ToListAsync();

    // Explicitly specify the path to the view located under Views/Manager/FinanceMenu.cshtml
    return View("~/Views/Manager/FinanceMenu.cshtml", studentFinance);
}

// Edit Action - GET method for displaying the form
public async Task<IActionResult> Edit(int id)
{
    var studentFinance = await _context.StudentFinances
        .FirstOrDefaultAsync(sf => sf.Id == id);

    if (studentFinance == null)
    {
        return NotFound();
    }

    return View("~/Views/Manager/Edit.cshtml", studentFinance);
}

// Edit Action - POST method for saving the changes
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,StudentID,TotalFees,AmountPaid,LastUpdated")] StudentFinance studentFinance)
{
    if (id != studentFinance.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
                        // Update LastUpdated field
            studentFinance.LastUpdated = DateTime.Now;


            _context.Update(studentFinance);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.StudentFinances.Any(sf => sf.Id == studentFinance.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(FinanceMenu)); // Redirect back to the finance menu
    }
    return View("~/Views/Manager/FinanceMenu.cshtml", studentFinance);
}


}