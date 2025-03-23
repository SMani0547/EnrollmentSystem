using System;

namespace USPSystem.Models;

public class GradeViewModel
{
    public string StudentId { get; set; }
    public string CourseCode { get; set; }
    public string CourseName { get; set; }
    public string Grade { get; set; }
    public int Year { get; set; }
    public int Semester { get; set; }
    public DateTime GradedDate { get; set; }
} 

