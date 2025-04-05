using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using USPSystem.Models;
using USPSystem.Services;

namespace USPSystem.APIController;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class StudentFinanceController : ControllerBase
{
    private readonly IStudentFinanceService _financeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentFinanceController(IStudentFinanceService financeService, UserManager<ApplicationUser> userManager)
    {
        _financeService = financeService;
        _userManager = userManager;
    }

    // GET: api/studentfinance
    [HttpGet]
    public IActionResult Index()
    {
        // Redirects to finance menu - optional, or just remove and keep below route only.
        return RedirectToAction(nameof(FinanceMenu));
    }

    // GET: api/studentfinance/finance-menu
    [HttpGet("finance-menu")]
    public async Task<ActionResult<object>> FinanceMenu()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("User not found.");

        var studentFinance = await _financeService.GetStudentFinanceAsync(user.Id);

        if (studentFinance == null)
        {
            return Ok(new
            {
                NoFinanceRecord = true,
                Message = "No financial record found."
            });
        }

        return Ok(new
        {
            NoFinanceRecord = false,
            Data = studentFinance
        });
    }
}
