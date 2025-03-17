using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? StudentId { get; set; }
    
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
} 