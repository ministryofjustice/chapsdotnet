using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class TeamViewModel
    {
        [Key]
        public int TeamId { get; set; }
        [Required, MaxLength(10), Display(Name = "Team Acronym")]
        public string? Acronym { get; set; }
        [Required, MaxLength(40), Display(Name = "Team")]
        public string? Name { get; set; }
        [MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }
        [Display(Name = "OGD?")]
        public bool IsOgd { get; set; }
        [Display(Name = "Private Office?")]
        public bool IsPod { get; set; }
        public bool Active { get; set; }
        public DateTime? deactivated { get; set; }
        public string? deactivatedBy { get; set; }

    }
}
