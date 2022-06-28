namespace ChapsDotNET.Business.Models.Common
{
    public class SalutationRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}
