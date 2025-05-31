using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models;

public class GraduationApplication
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string StudentId { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    [Required]
    public string PostalAddress { get; set; }
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string Telephone { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Programme { get; set; }
    
    public string MajorI { get; set; }
    
    public string? MajorII { get; set; }
    
    public string? Minor { get; set; }
    
    [Required]
    public string GraduationCeremony { get; set; }
    
    [Required]
    public bool WillAttend { get; set; }
    
    [Required]
    public bool DeclarationConfirmed { get; set; }
    
    public DateTime ApplicationDate { get; set; } = DateTime.Now;
    
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    
    // New field for admin comments
    public string? AdminComments { get; set; }
}

public enum ApplicationStatus
{
    Pending,
    Approved,
    Rejected,
    Deferred
} 