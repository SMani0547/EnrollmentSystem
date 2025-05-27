using System;

namespace USPSystem.Models;

public class GradeViewModel
{
    public int Id { get; set; }
    public required string StudentId { get; set; }
    public required string CourseCode { get; set; }
    public required string CourseName { get; set; }
    public required string Grade { get; set; }
    public decimal? Marks { get; set; }
    public int Year { get; set; }
    public int Semester { get; set; }
    public DateTime GradedDate { get; set; }
    public bool HasAppliedForRecheck { get; set; }
}

public enum GradeRecheckStatus
{
    Pending,
    InProgress, 
    Completed,
    Rejected
}