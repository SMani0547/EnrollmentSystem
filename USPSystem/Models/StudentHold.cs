using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSystem.Models
{
    public class StudentHold
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [Required]
        public string HoldType { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        public string? ResolvedByUserId { get; set; }
        public ApplicationUser? ResolvedByUser { get; set; }
    }
} 