using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

//TODO: Make an interface or generic model for the index views, so all admin indexes have sort options, pagination and alert section
namespace ChapsDotNET.Models
{
	public class SalutationsAdminViewModel
	{
		public Frontend.Components.Alert.AlertModel? Alert { get; set; }
        public PagedResult<List<SalutationModel>>? Salutations { get; set; }
	}
}
