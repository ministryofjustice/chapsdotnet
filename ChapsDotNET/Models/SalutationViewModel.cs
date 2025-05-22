using ChapsDotNET.Frontend.Components.Alert;
using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class SalutationViewModel
    {
        [Key]
        public int SalutationId { get; set; }
        [Required, MaxLength(10), Display(Name = "Title")]
        public string? Detail { get; set; }
        public bool Active { get; set; }
        public AlertModel? Alert { get; set; }
    }
}
