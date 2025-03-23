using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.Controllers;

[Authorize(Roles = "Student")]
public class StudentFinanceController : Controller
{
    private readonly IStudentFinanceService _financeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentFinanceController(IStudentFinanceService financeService, UserManager<ApplicationUser> userManager)
    {
        _financeService = financeService;
        _userManager = userManager;
    }

    public async Task<IActionResult> FinanceMenu()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        var studentFinance = await _financeService.GetStudentFinanceAsync(user.Id);

        if (studentFinance == null)
        {
            // Pass a flag to the view to indicate no financial record
            ViewBag.NoFinanceRecord = true;
            return View();
        }

        // Pass the finance details to the view
        ViewBag.NoFinanceRecord = false;
        return View(studentFinance);
    }
} 