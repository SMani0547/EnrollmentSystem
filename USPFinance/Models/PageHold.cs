using System;
using System.ComponentModel.DataAnnotations;

namespace USPFinance.Models
{
    public class PageHold
    {
        public int Id { get; set; }

        [Required]
        public string PageName { get; set; }

        [Required]
        public string PagePath { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string LastModifiedBy { get; set; }
    }
} 