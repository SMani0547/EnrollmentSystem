using System;
using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models;

public class RecheckApplicationModel
{
    public int GradeId { get; set; }

    [Required]
    public string CourseCode { get; set; }
    
    [Required]
    public string CourseName { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public int Semester { get; set; }
    
    [Required]
    public string CurrentGrade { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Contact Email")]
    public string Email { get; set; }
    
    [Required]
    [Display(Name = "Payment Receipt Number")]
    public string PaymentReceiptNumber { get; set; }
    
    [Required]
    [StringLength(500, MinimumLength = 50)]
    [Display(Name = "Reason for Recheck")]
    public string Reason { get; set; }
    
    [Display(Name = "Additional Comments")]
    [StringLength(1000)]
    public string? AdditionalComments { get; set; }
    
    [Required]
    [Display(Name = "Declaration")]
    public bool AgreesToTerms { get; set; }

    // Payment amount for display purposes
    public const decimal RecheckFee = 50.00m;
} 