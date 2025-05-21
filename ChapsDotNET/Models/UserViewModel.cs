using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Data.Entities;
using ChapsDotNET.Frontend.Components.Alert;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Models
{
    public class UserViewModel
    {
        [Key]
        public int UserId { get; set; }
        // Name has been re-labelled as Email as it must be a valid email address for authentication.
        [Required(ErrorMessage = "Enter an email address"), MaxLength(50), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Enter a display name"), MaxLength(50)]
        public string? DisplayName { get; set; }

        [MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Select a team")]
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
        [Required(ErrorMessage = "Select a role")]
        public int RoleStrength { get; set; }

        public SelectList? TeamList { get; set; }
        public SelectList? RoleList { get; set; }
        public AlertModel? Alert { get; set; }

    }
}

