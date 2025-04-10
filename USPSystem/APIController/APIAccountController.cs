using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using USPSystem.Models;
using USPSystem.Models.ViewModels;

namespace USPSystem.APIController;

/// <summary>
/// API controller for account-related operations such as login, registration, and profile management
/// </summary>
[ApiController] // Indicates that this is an API controller
[Route("api/[controller]")] // Defines the route for the controller (e.g., api/account)
public class APIAccountController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<APIAccountController> _logger;

    // Constructor to initialize dependencies
    public APIAccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<APIAccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and creates a session
    /// </summary>
    /// <param name="model">Login credentials including username/email and password</param>
    /// <returns>Success status and message</returns>
    /// <response code="200">Returns success message if login is successful</response>
    /// <response code="400">If the login data is invalid</response>
    /// <response code="401">If the credentials are incorrect</response>
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

    /// <summary>
    /// Logs out the current user and ends their session
    /// </summary>
    /// <returns>Success message</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="401">If the user is not authenticated</response>
    [Authorize] // Requires authentication
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return Ok(new { message = "Logout successful" }); // Success response after logging out
    }

    /// <summary>
    /// Registers a new user account
    /// </summary>
    /// <param name="model">User registration details</param>
    /// <returns>Success message if registration is successful</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="400">If the registration data is invalid</response>
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

    /// <summary>
    /// Retrieves the current user's profile information
    /// </summary>
    /// <returns>User profile details</returns>
    /// <response code="200">Returns the user profile</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the user is not found</response>
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

    /// <summary>
    /// Updates the current user's profile information
    /// </summary>
    /// <param name="firstName">User's first name</param>
    /// <param name="lastName">User's last name</param>
    /// <param name="majorI">Primary major</param>
    /// <param name="majorII">Secondary major (if applicable)</param>
    /// <param name="minorI">Minor (if applicable)</param>
    /// <param name="majorType">Type of major program</param>
    /// <returns>Success message if update is successful</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="400">If the update data is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the user is not found</response>
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
