using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class ApplicationUser : IdentityUser
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
    [Display(Name = "Admission Year")]
    [Range(2020, 2100)]
    public int AdmissionYear { get; set; }

    [Required]
    [Display(Name = "Major I")]
    [StringLength(10)]
    public required string MajorI { get; set; }

    [Display(Name = "Major II")]
    [StringLength(10)]
    public string? MajorII { get; set; }

    [Display(Name = "Minor I")]
    [StringLength(10)]
    public string? MinorI { get; set; }

    [Required]
    [Display(Name = "Major Type")]
    public MajorType MajorType { get; set; }

    public virtual ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
}

public enum MajorType
{
    SingleMajor,
    DoubleMajor,
    MajorMinor
} 