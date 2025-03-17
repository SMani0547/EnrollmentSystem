using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class Course
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(10)]
    public string CourseCode { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public int Credits { get; set; }
    
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
} 