namespace ChapsDotNET.Business.Models.Common
{
    public class UserRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
        public string? DisplayNameFilterTerm { get; set; }
        public string? RoleLevelFilterTerm { get; set; }
        public string? SortOrder { get; set; }
    }
}
