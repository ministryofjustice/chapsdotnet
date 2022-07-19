using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel
    {
        public int PublicHolidayID { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
