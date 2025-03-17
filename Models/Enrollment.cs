using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPEducation.Models;

public class Enrollment
{
    public int Id { get; set; }
    
    [Required]
    public string StudentId { get; set; } = string.Empty;
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    public DateTime EnrollmentDate { get; set; }
    
    public EnrollmentStatus Status { get; set; }
    
    [ForeignKey("StudentId")]
    public virtual ApplicationUser Student { get; set; } = null!;
    
    [ForeignKey("CourseId")]
    public virtual Course Course { get; set; } = null!;
}

public enum EnrollmentStatus
{
    Pending,
    Approved,
    Rejected
} 