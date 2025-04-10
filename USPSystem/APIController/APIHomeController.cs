using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using USPSystem.Models;

namespace USPSystem.APIController;

/// <summary>
/// API controller for home and general application operations
/// </summary>
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

    /// <summary>
    /// Gets the initial application state and redirect information
    /// </summary>
    /// <returns>User role and redirect information</returns>
    /// <response code="200">Returns role and redirect information for authenticated users</response>
    /// <response code="401">If the user is not authenticated</response>
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

    /// <summary>
    /// Gets privacy information
    /// </summary>
    /// <returns>Privacy information</returns>
    /// <response code="200">Returns privacy information</response>
    /// <response code="401">If the user is not authenticated</response>
    [Authorize]
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return Ok(new { message = "Privacy information (static or future content)" });
    }

    /// <summary>
    /// Handles application errors
    /// </summary>
    /// <returns>Error information</returns>
    /// <response code="500">Returns error details</response>
    [HttpGet("error")]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return Problem(detail: "An error occurred.", instance: requestId);
    }
}
