using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models;

public class StudentFinance
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string StudentID { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Total Fees")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalFees { get; set; }

    [Required]
    [Display(Name = "Amount Paid")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }

    [Required]
    [Display(Name = "Outstanding Balance")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OutstandingBalance { get; set; }

    [ForeignKey("StudentID")]
    public virtual ApplicationUser Student { get; set; } = null!;
} 