using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.ComponentModel;
using ChapsDotNET.Common.Helpers;


namespace ChapsDotNET.Models
{
    public class ReportViewModel
    {
        [Key]
        public int ReportId { get; set; }
        [Required, MaxLength(40)]
        public string Name { get; set; } = string.Empty;

        public int MaxLengthShort { get; set; } = 200;
        public int MaxLengthLong { get; set; } = 1000;

        [MaxLength(200), DataType(DataType.MultilineText), Display(Name="Description")]
        public string? Description { get; set; }

        [MaxLength(1000), DataType(DataType.MultilineText), UIHint("TextAreaWithCountdown"),Display(Name = "Long Description")]
        public string? LongDescription { get; set; }

    }
}