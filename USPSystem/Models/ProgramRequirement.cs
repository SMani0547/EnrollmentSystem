using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models;

public class ProgramRequirement
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProgramId { get; set; }

    [Required]
    public int SubjectAreaId { get; set; }

    [Required]
    public RequirementType Type { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int CreditPointsRequired { get; set; }

    [StringLength(2)]
    public string? MinimumGrade { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(SubjectAreaId))]
    public required SubjectArea SubjectArea { get; set; }

    [ForeignKey(nameof(ProgramId))]
    public required AcademicProgram Program { get; set; }

    public required ICollection<Course> RequiredCourses { get; set; } = new List<Course>();
}

public enum RequirementType
{
    [Display(Name = "Major Core Course")]
    MajorCore,
    
    [Display(Name = "Major Elective")]
    MajorElective,
    
    [Display(Name = "Minor Core Course")]
    MinorCore,
    
    [Display(Name = "Minor Elective")]
    MinorElective,
    
    [Display(Name = "Progression Requirement")]
    ProgressionRequirement
} 

