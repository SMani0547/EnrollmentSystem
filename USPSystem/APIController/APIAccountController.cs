using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using USPSystem.Models;
using USPSystem.Models.ViewModels;

namespace USPSystem.APIController;

[ApiController] // Indicates that this is an API controller
[Route("api/[controller]")] // Defines the route for the controller (e.g., api/account)
public class AccountController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    // Constructor to initialize dependencies
    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    // Login action: Handles user login with username and password        // POST api/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data provided." });
            }

            string userName = model.Login;
            if (!userName.Contains("@"))
            {
                userName = userName.ToUpper(); // Convert student ID to uppercase
            }

            var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Ok(new { success = true, message = "Login successful!" });
            }

            if (result.RequiresTwoFactor)
            {
                return BadRequest(new { success = false, message = "Two-factor authentication required." });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return BadRequest(new { success = false, message = "Account locked out." });
            }
            else
            {
                return Unauthorized(new { success = false, message = "Invalid login attempt." });
            }
        }

    // Logout action: Handles user logout
    [Authorize] // Requires authentication
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return Ok(new { message = "Logout successful" }); // Success response after logging out
    }

    // Register action: Handles user registration (new account creation)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); // Returns 400 if model state is invalid

        var user = new ApplicationUser
        {
            UserName = model.StudentId.ToUpper(),
            StudentId = model.StudentId.ToUpper(),
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true,
            AdmissionYear = model.AdmissionYear,
            MajorType = model.MajorType,
            MajorI = model.MajorI,
            MajorII = model.MajorII,
            MinorI = model.MinorI
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Student"); // Assigning "Student" role
            await _signInManager.SignInAsync(user, isPersistent: false);

            _logger.LogInformation("User created a new account with password.");
            return Ok(new { message = "Registration successful" }); // Success response
        }

        return BadRequest(result.Errors.Select(e => e.Description)); // Return error if registration fails
    }

    // GetProfile action: Retrieves the user's profile details
    [Authorize] // Requires authentication
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound(new { message = "User not found" }); // User not found

        var model = new ProfileViewModel
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            MajorI = user.MajorI,
            AdmissionYear = user.AdmissionYear
        };

        return Ok(model); // Return profile data in response
    }

    // UpdateProfile action: Allows the user to update their profile
    [Authorize] // Requires authentication
    [HttpPost("profile/update")]
    public async Task<IActionResult> UpdateProfile([FromForm] string firstName, [FromForm] string lastName,
        [FromForm] string majorI, [FromForm] string? majorII, [FromForm] string? minorI, [FromForm] MajorType majorType)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound(new { message = "User not found" }); // User not found

        user.FirstName = firstName;
        user.LastName = lastName;
        user.MajorI = majorI;
        user.MajorII = majorII;
        user.MinorI = minorI;
        user.MajorType = majorType;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(new { message = "Profile updated successfully" }); // Success response
        }

        return BadRequest(result.Errors.Select(e => e.Description)); // Return error if update fails
    }

    // AccessDenied action: Returns an access denied response
    [HttpGet("accessdenied")]
    public IActionResult AccessDenied()
    {
        return Forbid("Access denied"); // Forbidden response for unauthorized access
    }
}
