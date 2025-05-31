using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models;

public class Grade
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string CourseId { get; set; } = string.Empty;

    [Required]
    public float Marks { get; set; }

    [Required]
    [MaxLength(2)]
    public string GradeLetter { get; set; } = string.Empty;
}

