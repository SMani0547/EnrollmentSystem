using System.ComponentModel.DataAnnotations;

namespace USPEducation.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Email or Student ID")]
    public string LoginIdentifier { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
} 