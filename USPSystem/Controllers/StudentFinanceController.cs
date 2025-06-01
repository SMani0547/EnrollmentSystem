using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using USPSystem.Models;
using USPSystem.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace USPSystem.Controllers;

[Authorize(Roles = "Student")]
public class StudentFinanceController : BaseController
{
    private readonly IStudentFinanceService _financeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<StudentFinanceController> _logger;

    public StudentFinanceController(
        IStudentFinanceService financeService, 
        UserManager<ApplicationUser> userManager, 
        StudentHoldService studentHoldService,
        PageHoldService pageHoldService,
        ILogger<StudentFinanceController> logger) : base(studentHoldService, pageHoldService, userManager)
    {
        _financeService = financeService;
        _userManager = userManager;
        _logger = logger;
    }

    // Adding Index action to handle the root controller URL
    public IActionResult Index()
    {
        return RedirectToAction(nameof(FinanceMenu));
    }

    public async Task<IActionResult> FinanceMenu()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        _logger.LogInformation("Fetching finance data for student: {StudentId}", user.StudentId);
        var studentFinance = await _financeService.GetStudentFinanceAsync(user.StudentId);

        if (studentFinance == null)
        {
            _logger.LogWarning("No finance record found for student: {StudentId}", user.StudentId);
            ViewBag.NoFinanceRecord = true;
            return View();
        }

        _logger.LogInformation("Finance data retrieved - TotalFees: {TotalFees}, AmountPaid: {AmountPaid}, Outstanding: {Outstanding}", 
            studentFinance.TotalFees, 
            studentFinance.AmountPaid, 
            studentFinance.TotalFees - studentFinance.AmountPaid);

        // Pass the finance details to the view
        ViewBag.NoFinanceRecord = false;
        return View(studentFinance);
    }
} 