using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Attributes;
using ChapsDotNET.Common.DateValidation;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel
    {
        public int PublicHolidayID { get; set; }
        [Required, FutureDate(ErrorMessage="Holiday date must be in the future"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required, MaxLength(30)]
        public string? Description { get; set; }
    }
}
