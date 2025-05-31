using System;
using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models
{
    public class RecheckRequestViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }
        
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        
        [Display(Name = "Current Grade")]
        public string CurrentGrade { get; set; }
        
        [Display(Name = "Year")]
        public int Year { get; set; }
        
        [Display(Name = "Semester")]
        public int Semester { get; set; }
        
        [Display(Name = "Application Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime ApplicationDate { get; set; }
        
        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [Display(Name = "Reason")]
        public string Reason { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Payment Receipt")]
        public string PaymentReceiptNumber { get; set; }
    }
} 