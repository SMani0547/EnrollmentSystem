using System.ComponentModel.DataAnnotations;

namespace USPSystem.Models;

public enum EnrollmentStatus
{
    [Display(Name = "Pending")]
    Pending,
    [Display(Name = "Approved")]
    Approved,
    [Display(Name = "Rejected")]
    Rejected,
    [Display(Name = "Enrolled")]
    Enrolled,
    [Display(Name = "Withdrawn")]
    Withdrawn,
    [Display(Name = "Completed")]
    Completed,
    [Display(Name = "Failed")]
    Failed
} 

