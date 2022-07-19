using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class SalutationViewModel
    {
        public int SalutationId { get; set; }
        [Required, MaxLength(10), Display(Name = "Title")]
        public string? Detail { get; set; }
        public bool Active { get; set; }

    }
}
