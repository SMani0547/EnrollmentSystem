using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPFinance.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string TransactionType { get; set; } // Income or Expense

        public string? Category { get; set; }

        public string? Notes { get; set; }
    }
} 