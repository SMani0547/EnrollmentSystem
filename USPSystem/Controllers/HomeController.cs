using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using USPSystem.Models;
using USPSystem.Services;
using System.Threading.Tasks;

namespace USPSystem.Controllers;

public class HomeController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        StudentHoldService studentHoldService,
        PageHoldService pageHoldService,
        UserManager<ApplicationUser> userManager,
        ILogger<HomeController> logger)
        : base(studentHoldService, pageHoldService, userManager)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Program", new { area = "Manager" });
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
        }

        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


