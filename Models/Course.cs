using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class Course
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(10)]
    [Display(Name = "Course Code")]
    public string CourseCode { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [Range(0, 120)]
    public int Credits { get; set; }

    public decimal Fee { get; set; }
    
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
} 