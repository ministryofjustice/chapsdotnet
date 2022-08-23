namespace ChapsDotNET.Business.Models.Common
{
    public class MPsRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}
