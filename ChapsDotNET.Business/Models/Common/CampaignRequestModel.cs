namespace ChapsDotNET.Business.Models.Common
{
    public class CampaignRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}
