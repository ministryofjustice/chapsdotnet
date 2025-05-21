using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Models
{
	public class UserAdminViewModel
	{
		public string? SortOrder { get; set; }
		public Frontend.Components.Alert.AlertModel? Alert { get; set; }
        public PagedResult<List<UserModel>>? Users { get; set; }
	}
}

