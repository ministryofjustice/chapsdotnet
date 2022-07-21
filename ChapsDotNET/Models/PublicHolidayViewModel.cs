using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel
    {
        public int PublicHolidayID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required, MaxLength(30)           ]
        public string? Description { get; set; }
    }
}
