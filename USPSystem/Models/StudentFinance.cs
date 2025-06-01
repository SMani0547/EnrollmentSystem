using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models;

public class StudentFinance
{
    public int Id { get; set; }
    public string StudentID { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalFees { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
    
    public DateTime LastUpdated { get; set; }

    // Hold related properties
    public bool IsOnHold { get; set; }
    public string HoldReason { get; set; } = string.Empty;
    public DateTime? HoldStartDate { get; set; }
    public DateTime? HoldEndDate { get; set; }
    public string HoldPlacedBy { get; set; } = string.Empty;  // To track which manager placed the hold

    [ForeignKey("StudentID")]
    public virtual ApplicationUser Student { get; set; } = null!;
} 