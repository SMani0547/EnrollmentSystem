using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPEducation.Models;

public class Enrollment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(450)]
    public string StudentId { get; set; } = string.Empty;
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    public string Semester { get; set; } = string.Empty;
    
    [Required]
    public int Year { get; set; }
    
    public string? Grade { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public virtual ApplicationUser Student { get; set; } = null!;
    
    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
}

public enum EnrollmentStatus
{
    Pending,
    Approved,
    Rejected
} 