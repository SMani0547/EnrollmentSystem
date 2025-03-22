using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    public int AdmissionYear { get; set; }

    [Required]
    public MajorType MajorType { get; set; }

    [Required]
    [StringLength(10)]
    public string MajorI { get; set; }

    [StringLength(10)]
    public string? MajorII { get; set; }

    [StringLength(10)]
    public string? MinorI { get; set; }

    public ICollection<StudentEnrollment> Enrollments { get; set; }
}

public enum MajorType
{
    SingleMajor,
    DoubleMajor,
    MajorMinor
} 