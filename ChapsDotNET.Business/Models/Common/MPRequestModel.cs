namespace ChapsDotNET.Business.Models.Common
{
    public class MPRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
        public string nameFilterTerm { get; set; } = String.Empty;
        public string addressFilterTerm { get; set; } = String.Empty;
        public string emailFilterTerm { get; set; } = String.Empty;
        public string? sortOrder { get; set; }
        public bool activeFilter { get; set; } = true;
    }
}
