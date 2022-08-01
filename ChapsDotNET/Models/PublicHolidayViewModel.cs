using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Common.DateValidation;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel
    {
        public int PublicHolidayID { get; set; }
        [Required, CustomDate(ErrorMessage="The holiday date must be in the future"), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required, MaxLength(30)]
        public string? Description { get; set; }
    }
}
