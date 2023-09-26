using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class LeadSubjectViewModel
    {
        public int LeadSubjectId { get; set; }
        [Required, MaxLength(100), Display(Name = "Detail")]
        public string? Detail { get; set; }
        public bool Active { get; set; }
        public DateTime? deactivated { get; set; }
        public string? deactivatedBy { get; set; }
    }
}
