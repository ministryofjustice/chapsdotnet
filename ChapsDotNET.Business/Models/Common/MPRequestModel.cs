namespace ChapsDotNET.Business.Models.Common
{
    public class MPRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; }
        public string? nameFilterTerm { get; set; } 
        public string? addressFilterTerm { get; set; }
        public string? emailFilterTerm { get; set; } 
        public string? sortOrder { get; set; }
        public string? SortColumn { get; set; }
        //public bool activeFilter { get; set; }
    }
}
