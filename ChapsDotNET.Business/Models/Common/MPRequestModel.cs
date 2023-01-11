using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Business.Models.Common
{
    public class MPRequestModel : PagedRequest
    {
        public bool ActiveFilter { get; set; } = true;
        public string? AddressFilterTerm { get; set; } = String.Empty;
        public string? EmailFilterTerm { get; set; } = String.Empty;
        public string? NameFilterTerm { get; set; } = String.Empty;
        public string? SortOrder { get; set; }
    }
}
