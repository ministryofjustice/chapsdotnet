using System;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Models
{
	public class UserViewModel
	{

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }

        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public int RoleStrength { get; set; }

        public SelectList? TeamList { get; set; }
        public SelectList? RoleList { get; set; }

    }
}

