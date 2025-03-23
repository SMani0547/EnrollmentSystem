using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Student ID or Email")]
    public required string Login { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
} 

