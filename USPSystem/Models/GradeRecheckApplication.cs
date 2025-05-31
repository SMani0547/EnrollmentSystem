using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models
{
    // This model matches the RecheckApplication table in the USPGradeSystemDB
    public class GradeRecheckApplication
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string StudentId { get; set; } = string.Empty;
        
        [Required]
        public string CourseCode { get; set; } = string.Empty;
        
        [Required]
        public string CourseName { get; set; } = string.Empty;
        
        [Required]
        public string CurrentGrade { get; set; } = string.Empty;
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public int Semester { get; set; }
        
        [Required]
        public DateTime ApplicationDate { get; set; }
        
        [Required]
        public int Status { get; set; } // Corresponds to GradeRecheckStatus enum
        
        [Required]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PaymentReceiptNumber { get; set; } = string.Empty;
        
        [Required]
        public string Reason { get; set; } = string.Empty;
        
        public string? AdditionalComments { get; set; }
    }
} 