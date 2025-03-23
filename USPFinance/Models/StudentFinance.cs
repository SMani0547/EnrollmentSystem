using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPFinance.Models
{
    public class StudentFinance
    {
        public int Id { get; set; }
        public string StudentID { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalFees { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        
        public DateTime LastUpdated { get; set; }
    }
} 