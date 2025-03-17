using System.ComponentModel.DataAnnotations;

namespace USPEducation.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter your Student ID or Email")]
    [Display(Name = "Student ID or Email")]
    public string LoginIdentifier { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
} 