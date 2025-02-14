using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Models
{
	public class UserViewModel
	{
        [Key]
        public int UserId { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        [Required, MaxLength(50)]
        public string? DisplayName { get; set; }

        [MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }

        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public int RoleStrength { get; set; }

        public SelectList? TeamList { get; set; }
        public SelectList? RoleList { get; set; }

        public string? Warning { get; set; }

    }
}

