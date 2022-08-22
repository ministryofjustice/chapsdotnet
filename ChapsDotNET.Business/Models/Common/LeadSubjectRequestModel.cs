namespace ChapsDotNET.Business.Models.Common
{
    public class LeadSubjectRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}

