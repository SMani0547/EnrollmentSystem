using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPEducation.Models
{
    public class StudentFinance
    {
        [Key] // This marks the primary key
        public int Id { get; set; } 
        public string StudentID { get; set; } = string.Empty;
        public decimal TotalFees { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal OutstandingBalance => TotalFees - AmountPaid;
        public DateTime LastUpdated { get; set; }
    }
}