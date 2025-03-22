using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPEducation.Models;

public class AcademicProgram
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    [Display(Name = "Program Code")]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = "Program Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Total Credit Points")]
    [Range(0, 480)]
    public int CreditPoints { get; set; }

    [Required]
    [Display(Name = "Duration (Years)")]
    [Range(1, 6)]
    public int Duration { get; set; }

    [Required]
    [Display(Name = "Level")]
    public ProgramLevel Level { get; set; }

    [Required]
    [Display(Name = "Offering Year")]
    [Range(2024, 2100)]
    public int OfferingYear { get; set; }

    [Required]
    [Display(Name = "Major Credits Required")]
    [Range(0, 480)]
    public int MajorCreditsRequired { get; set; } = 48;

    [Required]
    [Display(Name = "Minor Credits Required")]
    [Range(0, 480)]
    public int MinorCreditsRequired { get; set; } = 24;

    [Required]
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<ProgramRequirement> Requirements { get; set; } = new List<ProgramRequirement>();
    public virtual ICollection<Course> CoreCourses { get; set; } = new List<Course>();
    public virtual ICollection<Course> ElectiveCourses { get; set; } = new List<Course>();
    public virtual ICollection<StudentEnrollment> StudentEnrollments { get; set; } = new List<StudentEnrollment>();
}

public enum ProgramLevel
{
    [Display(Name = "Bachelor of Arts")]
    BA,
    [Display(Name = "Bachelor of Commerce")]
    BCom,
    [Display(Name = "Bachelor of Science")]
    BSc,
    [Display(Name = "Postgraduate Diploma")]
    PGDip,
    [Display(Name = "Master's Degree")]
    Masters,
    [Display(Name = "Doctor of Philosophy")]
    PhD
} 