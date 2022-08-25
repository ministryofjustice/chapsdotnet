using System;
namespace ChapsDotNET.Business.Models.Common
{
	public class MoJMinisterRequestModel : PagedRequest
	{
		public bool ShowActiveAndInactive { get; set; } = false;
	}
}
