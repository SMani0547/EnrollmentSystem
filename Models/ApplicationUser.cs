using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [StringLength(100)]
    [Display(Name = "Middle Name")]
    public string? MiddleName { get; set; }
    
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(10)]
    [Display(Name = "Student ID")]
    public string? StudentId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    [StringLength(100)]
    public string? Citizenship { get; set; }

    [StringLength(100)]
    [Display(Name = "Program")]
    public string? Program { get; set; }

    [StringLength(50)]
    [Display(Name = "Student Level")]
    public string? StudentLevel { get; set; }

    [StringLength(100)]
    [Display(Name = "Campus")]
    public string? StudentCampus { get; set; }

    [StringLength(100)]
    [Display(Name = "Exam Site")]
    public string? ExamSite { get; set; }

    [StringLength(100)]
    [Display(Name = "Major I")]
    public string? MajorI { get; set; }

    [StringLength(100)]
    [Display(Name = "Major II")]
    public string? MajorII { get; set; }

    [StringLength(100)]
    [Display(Name = "Minor I")]
    public string? MinorI { get; set; }

    [StringLength(100)]
    [Display(Name = "Minor II")]
    public string? MinorII { get; set; }

    [StringLength(50)]
    [Display(Name = "Major Type")]
    public string? MajorType { get; set; }

    [StringLength(255)]
    [Display(Name = "Profile Photo")]
    public string? ProfilePhotoUrl { get; set; }

    // ELSA Test Results
    [StringLength(50)]
    [Display(Name = "ELSA Test Result")]
    public string? ElsaTestResult { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "ELSA Test Date")]
    public DateTime? ElsaTestDate { get; set; }

    // GSD Test Results
    [StringLength(50)]
    [Display(Name = "GSD Test Result")]
    public string? GsdTestResult { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "GSD Test Date")]
    public DateTime? GsdTestDate { get; set; }

    // Passport & Visa
    [StringLength(50)]
    [Display(Name = "Passport Number")]
    public string? PassportNumber { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Passport Expiry Date")]
    public DateTime? PassportExpiryDate { get; set; }

    [StringLength(50)]
    [Display(Name = "Visa Number")]
    public string? VisaNumber { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Visa Expiry Date")]
    public DateTime? VisaExpiryDate { get; set; }

    // Navigation Properties
    public virtual StudentAddress? StudentAddress { get; set; }
    public virtual EmergencyContact? EmergencyContact { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
} 