using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using USPSystem.Models;

namespace USPSystem.APIController;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    // GET: api/home
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new { message = "User not found" });

            if (User.IsInRole("Manager"))
            {
                return Ok(new { role = "Manager", redirect = "/manager/program" });
            }
            else if (User.IsInRole("Student"))
            {
                return Ok(new { role = "Student", redirect = "/student" });
            }
        }

        return Unauthorized(new { message = "User not authenticated", redirect = "/account/login" });
    }

    // GET: api/home/privacy
    [Authorize]
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return Ok(new { message = "Privacy information (static or future content)" });
    }

    // GET: api/home/error
    [HttpGet("error")]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return Problem(detail: "An error occurred.", instance: requestId);
    }
}
