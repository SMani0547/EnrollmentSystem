using System;
using System.ComponentModel.DataAnnotations;

namespace USPGradeSystem.Models
{
    public class RecheckApplication
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string StudentId { get; set; }
        
        [Required]
        public string CourseCode { get; set; }
        
        [Required]
        public string CourseName { get; set; }
        
        [Required]
        public string CurrentGrade { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public int Semester { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PaymentReceiptNumber { get; set; }
        
        [Required]
        public string Reason { get; set; }
        
        public string? AdditionalComments { get; set; }
        
        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public RecheckStatus Status { get; set; } = RecheckStatus.Pending;
    }

    public enum RecheckStatus
    {
        Pending,
        InProgress,
        Completed,
        Rejected
    }
} 