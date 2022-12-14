namespace ChapsDotNET.Business.Models.Common
{
    public class MPRequestModel : PagedRequest
    {
        public bool activeFilter { get; set; } = true;
        public bool ShowActiveAndInactive { get; set; } = false;
        public string addressFilterTerm { get; set; } = String.Empty;
        public string emailFilterTerm { get; set; } = String.Empty;
        public string nameFilterTerm { get; set; } = String.Empty;
        public string? sortOrder { get; set; }
    }
}
