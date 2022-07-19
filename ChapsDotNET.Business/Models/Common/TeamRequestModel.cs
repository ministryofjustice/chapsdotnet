namespace ChapsDotNET.Business.Models.Common
{
    public class TeamRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}
