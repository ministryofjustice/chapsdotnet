namespace ChapsDotNET.Business.Models.Common
{
    public class MPRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
        public string nameSearchTerm { get; set; } = String.Empty;
        public string addressSearchTerm { get; set; } = String.Empty;
        public string emailSearchTerm { get; set; } = String.Empty;
    }
}
