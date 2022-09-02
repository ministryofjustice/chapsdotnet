using System.Diagnostics.Metrics;
using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class MP : LookUpModel, IAuditable
    {
        /*
        MPID - Primary Key
        salutationID - Foreign Key   
        Surname
        FirstNames
        AddressLine1
        AddressLine2
        AddressLine3
        Town
        County
        Postcode
        Email
        RtHon
        Suffix
        Active - (not implemented)
        deactivatedOn - (not implemented)
        deactivatedByID - Foreign Key (not implemented)
        */

        public int MPID { get; set; }
        public int salutationID { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string? FirstNames { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string? Postcode { get; set; }
        public string? Email { get; set; }
        public bool RtHon { get; set; }
        public string? Suffix { get; set; }
        public bool Auditable()
        {
            return true;
        }
    }
}
