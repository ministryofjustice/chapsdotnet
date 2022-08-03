using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Models
{
    public class PublicHolidayViewModel : IValidatableObject
    {
        public int PublicHolidayID { get; set; }
        [BindProperty, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
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