using System;
using ChapsDotNET.Business.Models;

namespace ChapsDotNET.Models
{
	public class UsersViewModel
	{
		public string? SortOrder { get; set; }
		public List<UserModel>? Users { get; set; }
	}
}

