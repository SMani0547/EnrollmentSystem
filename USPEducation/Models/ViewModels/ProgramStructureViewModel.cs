using System.ComponentModel.DataAnnotations;

namespace USPEducation.Models.ViewModels
{
    public class ProgramStructureViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Program Code")]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Program Name")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Credit Points")]
        public int CreditPoints { get; set; }
        
        [Required]
        [Display(Name = "Duration (Years)")]
        public int Duration { get; set; }
        
        [Required]
        [Display(Name = "Level")]
        public ProgramLevel Level { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [Required]
        public string ProgramName { get; set; } = string.Empty;

        [Required]
        public string ProgramCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Offering Year")]
        [Range(2024, 2100)]
        public int OfferingYear { get; set; }

        [Required]
        [Display(Name = "Major Credits Required")]
        [Range(0, 480)]
        public int MajorCreditsRequired { get; set; }

        [Required]
        [Display(Name = "Minor Credits Required")]
        [Range(0, 480)]
        public int MinorCreditsRequired { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public ICollection<SubjectArea> AvailableMajors { get; set; } = new List<SubjectArea>();
        public ICollection<SubjectArea> AvailableMinors { get; set; } = new List<SubjectArea>();
        public ICollection<ProgramRequirement> Requirements { get; set; } = new List<ProgramRequirement>();
    }
} 