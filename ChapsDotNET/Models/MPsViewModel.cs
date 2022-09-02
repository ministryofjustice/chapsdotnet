using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class MPsViewModel
    {
        public int MPId { get; set; }
        public int SalutationId { get; set; }
        public string? Surname { get; set; }
        public string? FirstNames { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string? Postcode { get; set; }
        public string? Email { get; set; }
        public string? RtHon { get; set; }
        public string? Suffix { get; set; }
        public bool Active { get; set; }
    }
}
