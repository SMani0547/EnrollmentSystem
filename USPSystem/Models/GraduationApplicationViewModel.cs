using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models;

public class GraduationApplicationViewModel
{
    public string StudentId { get; set; }
    
    public string FirstName { get; set; }
    
    public string Surname { get; set; }
    
    [Required(ErrorMessage = "Postal address is required")]
    public string PostalAddress { get; set; }
    
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Telephone number is required")]
    public string Telephone { get; set; }
    
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }
    
    public string Programme { get; set; }
    
    public string MajorI { get; set; }
    
    public string? MajorII { get; set; }
    
    public string? Minor { get; set; }
    
    [Required(ErrorMessage = "Please select a graduation ceremony")]
    public string GraduationCeremony { get; set; }
    
    [Required(ErrorMessage = "Please indicate whether you will attend the ceremony")]
    public string AttendanceOption { get; set; }
    
    [Required(ErrorMessage = "You must confirm the declaration")]
    public bool ConfirmDeclaration { get; set; }
    
    // Helper property to convert AttendanceOption to boolean
    public bool WillAttend => AttendanceOption == "attend";
} 