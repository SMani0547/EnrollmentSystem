using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models.ViewModels;

public class ProfileViewModel
{
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Major")]
    public string MajorI { get; set; } = string.Empty;

    [Display(Name = "Admission Year")]
    public int AdmissionYear { get; set; }
} 