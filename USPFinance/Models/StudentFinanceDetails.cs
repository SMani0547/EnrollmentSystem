using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPFinance.Models
{
    public class StudentFinanceDetails
    {
        public int Id { get; set; }
        public string StudentID { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalFees { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal OutstandingBalance { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public List<PaymentRecord> PaymentHistory { get; set; } = new List<PaymentRecord>();
    }

    public class PaymentRecord
    {
        public DateTime Date { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public string Description { get; set; }
        
        public string? Notes { get; set; }
    }
} 