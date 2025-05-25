using System;
using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models;

public class RecheckApplicationModel
{
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
    [StringLength(500, MinimumLength = 50)]
    [Display(Name = "Reason for Recheck")]
    public string Reason { get; set; }
    
    [Display(Name = "Additional Comments")]
    [StringLength(1000)]
    public string? AdditionalComments { get; set; }
    
    [Required]
    [Display(Name = "Declaration")]
    public bool AgreesToTerms { get; set; }
} 