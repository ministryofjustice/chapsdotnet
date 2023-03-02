namespace ChapsDotNET.Business.Models.Common
{
    public class AlertRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}

