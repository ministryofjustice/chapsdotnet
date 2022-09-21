using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class ReportViewModel
    {
        [Key]
        public int ReportId { get; set; }
        [Required, MaxLength(40)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Display(Name = "Long Description")]
        public string? LongDescription { get; set; } 
    }
}