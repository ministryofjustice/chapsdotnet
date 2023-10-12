using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class MP : IAuditable
    {
        public bool Auditable() { return true; }
        public bool RtHon { get; set; }
        public int MPID { get; set; }
        public int? SalutationID { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? County { get; set; }
        public string? Email { get; set; }
        public string? FirstNames { get; set; }
        public string? Postcode { get; set; }
        public string? Suffix { get; set; }
        public string? Town { get; set; }
        public bool active { get; set; }
        public DateTime? deactivatedOn { get; set; }
        public int? deactivatedByID { get; set; }
    }
}
