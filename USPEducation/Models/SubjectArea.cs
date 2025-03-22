using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class SubjectArea
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(3)]
    [Display(Name = "Subject Code")]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    [Display(Name = "Subject Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Can be Major")]
    public bool CanBeMajor { get; set; } = true;

    [Required]
    [Display(Name = "Can be Minor")]
    public bool CanBeMinor { get; set; } = true;

    [Required]
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<ProgramRequirement> Requirements { get; set; } = new List<ProgramRequirement>();
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
} 