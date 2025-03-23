using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models.ViewModels;

public class RegisterViewModel
{
    [Required]
    [StringLength(20)]
    [Display(Name = "Student ID")]
    [RegularExpression(@"^[Ss]\d{8}$", ErrorMessage = "Student ID must be in the format S12345678")]
    public required string StudentId { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "Admission Year")]
    [Range(2020, 2100)]
    public int AdmissionYear { get; set; }

    [Required]
    [Display(Name = "Major I")]
    public required string MajorI { get; set; }

    [Display(Name = "Major II")]
    public string? MajorII { get; set; }

    [Display(Name = "Minor I")]
    public string? MinorI { get; set; }

    [Required]
    [Display(Name = "Major Type")]
    public MajorType MajorType { get; set; }
}

