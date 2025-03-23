namespace USPEducation.Models;

public class Grade
{

    public int Id { get; set; }

    public string StudentId { get; set; } = string.Empty;

    public string CourseId { get; set; } = string.Empty;

    public float Marks { get; set; }
    public string GradeLetter { get; set; } = string.Empty;


}