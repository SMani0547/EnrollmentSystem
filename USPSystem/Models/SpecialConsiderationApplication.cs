using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models
{
    public class SpecialConsiderationApplication
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string StudentId { get; set; }
        
        [Required]
        public required string LastName { get; set; }
        
        [Required]
        public required string FirstName { get; set; }
        
        public string? MiddleName { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public required string Campus { get; set; }
        
        [Required]
        public required string Address { get; set; }
        
        [Required]
        public required string SemesterYear { get; set; }
        
        [Required]
        [Phone]
        public required string Telephone { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        // Exam details (up to 6 courses)
        public string? CourseCode1 { get; set; }
        public DateTime? ExamDate1 { get; set; }
        public string? ExamTime1 { get; set; }
        
        public string? CourseCode2 { get; set; }
        public DateTime? ExamDate2 { get; set; }
        public string? ExamTime2 { get; set; }
        
        public string? CourseCode3 { get; set; }
        public DateTime? ExamDate3 { get; set; }
        public string? ExamTime3 { get; set; }
        
        public string? CourseCode4 { get; set; }
        public DateTime? ExamDate4 { get; set; }
        public string? ExamTime4 { get; set; }
        
        public string? CourseCode5 { get; set; }
        public DateTime? ExamDate5 { get; set; }
        public string? ExamTime5 { get; set; }
        
        public string? CourseCode6 { get; set; }
        public DateTime? ExamDate6 { get; set; }
        public string? ExamTime6 { get; set; }
        
        // Application type
        [Required]
        public SpecialConsiderationType ApplicationType { get; set; }
        
        [Required]
        public required string Reason { get; set; }
        
        // Supporting documents
        public string? SupportingDocuments { get; set; }
        
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        
        public string ApplicationStatus { get; set; } = "Pending";
        
        // Navigation property
        [ForeignKey("StudentId")]
        public virtual ApplicationUser? Student { get; set; }
    }
    
    public enum SpecialConsiderationType
    {
        CompassionatePass,
        AegrotatPass,
        SpecialExamination
    }
} 