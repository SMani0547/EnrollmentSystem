using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPEducation.Models;

public class StudentEnrollment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Student")]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course")]
    public int CourseId { get; set; }

    [Required]
    [Display(Name = "Year")]
    [Range(2024, 2100)]
    public int Year { get; set; }

    [Required]
    [Display(Name = "Semester")]
    public Semester Semester { get; set; }

    [StringLength(2)]
    [Display(Name = "Grade")]
    public string? Grade { get; set; }

    [Required]
    [Display(Name = "Status")]
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;

    [Required]
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(StudentId))]
    [Display(Name = "Student")]
    public virtual ApplicationUser Student { get; set; } = null!;

    [ForeignKey(nameof(CourseId))]
    [Display(Name = "Course")]
    public virtual Course Course { get; set; } = null!;
} 