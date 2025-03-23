using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models;

public class Course
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(10)]
    [Display(Name = "Course Code")]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    [Display(Name = "Course Name")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 48)]
    [Display(Name = "Credit Points")]
    public int CreditPoints { get; set; }
    
    [Required]
    [Display(Name = "Level")]
    public CourseLevel Level { get; set; }

    [Required]
    [Display(Name = "Semester")]
    public Semester Semester { get; set; }
    
    [Required]
    [Display(Name = "Subject Area")]
    public int SubjectAreaId { get; set; }
    
    [ForeignKey(nameof(SubjectAreaId))]
    [Display(Name = "Subject Area")]
    public virtual SubjectArea SubjectArea { get; set; } = null!;
    
    [Display(Name = "Prerequisites")]
    public virtual ICollection<Course> Prerequisites { get; set; } = new List<Course>();
    
    [Display(Name = "Is Prerequisite For")]
    public virtual ICollection<Course> IsPrerequisiteFor { get; set; } = new List<Course>();
    
    [Required]
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Course Fees")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "Course fees must be a positive value")]
    public decimal? Fees { get; set; }
    
    // Navigation properties for program relationships
    public virtual ICollection<AcademicProgram> IsCoreCourseFor { get; set; } = new List<AcademicProgram>();
    public virtual ICollection<AcademicProgram> IsElectiveCourseFor { get; set; } = new List<AcademicProgram>();
    public virtual ICollection<StudentEnrollment> StudentEnrollments { get; set; } = new List<StudentEnrollment>();
}

public enum Semester
{
    [Display(Name = "Semester 1")]
    Semester1,
    [Display(Name = "Semester 2")]
    Semester2,
    [Display(Name = "Summer")]
    Summer
}

public enum CourseLevel
{
    [Display(Name = "Level 100")]
    Level100,
    [Display(Name = "Level 200")]
    Level200,
    [Display(Name = "Level 300")]
    Level300,
    [Display(Name = "Level 400")]
    Level400,
    [Display(Name = "Level 500")]
    Level500,
    [Display(Name = "Level 600")]
    Level600,
    [Display(Name = "Level 700")]
    Level700,
    [Display(Name = "Level 800")]
    Level800,
    [Display(Name = "Level 900")]
    Level900
} 

