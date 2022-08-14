using System;
namespace ChapsDotNET.Business.Models.Common
{
    public class PublicHolidayRequestModel : PagedRequest
    {
        public bool ShowActiveAndInactive { get; set; } = false;
    }
}

