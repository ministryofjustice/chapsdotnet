using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Common.DateValidation;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel
    {
        public int PublicHolidayID { get; set; }
        [Required, CustomDate(ErrorMessage="The date must be in the future")]
        public DateTime Date { get; set; }
        [Required, MaxLength(30)]
        public string? Description { get; set; }
    }
}
