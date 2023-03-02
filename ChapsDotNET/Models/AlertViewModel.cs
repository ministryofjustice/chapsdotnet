using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class AlertViewModel
    {
        public int AlertId { get; set; }
        [Required, MaxLength(100), Display(Name = "Message")]
        public string? Message { get; set; }
        public bool live { get; set; }
    }
}
