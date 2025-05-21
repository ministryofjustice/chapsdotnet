namespace ChapsDotNET.Business.Models.Common
{
    public class UserRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
        public string? SortOrder { get; set; }
    }
}
