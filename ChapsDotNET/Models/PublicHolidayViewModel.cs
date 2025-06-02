using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel : IValidatableObject
    {
        private DateTime _date;

        public int PublicHolidayId { get; set; }
        [Required(ErrorMessage = "Enter a day"), Range(1, 31, ErrorMessage = "Day: Enter a value between 1 and 31")]
        public int Day
        {
            get => _date.Day;
            set => _date = new DateTime(Year, Month, value);
        }
        [Required(ErrorMessage = "Enter a month"), Range(1, 31, ErrorMessage = "Month: Enter a value between 1 and 12")]
        public int Month
        {
            get => _date.Month;
            set => _date = new DateTime(Year, value, Day);
        }
        [Required(ErrorMessage = "Enter a year")]
        public int Year
        {
            get => _date.Year;
            set => _date = new DateTime(value, Month, Day);
        }

        [BindProperty, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        [Required, MaxLength(30)]
        public string? Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date <= DateTime.Now)
            {
                yield return new ValidationResult("Date cannot be in the past");
            }
        }
    }
}