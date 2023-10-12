using System.ComponentModel.DataAnnotations;
using System.Security;
//using System.Xml.Linq;
using ChapsDotNET.Data.Entities;
//using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Business.Models
{
    public class MPModel
    {
        public bool Active { get; set; }
        public bool RtHon { get; set; }
        public int MPId { get; set; }
        public int? SalutationId { get; set; }
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
        public virtual Salutation Salutation { get; set; } = default!;
        public string? DisplayFullName { get; set; }
        public string? sortOrder { get; set; }
        public int? DeactivatedByID { get; set; }
        public DateTime? DeactivatedOn { get; set; }
        public string? DeactivatedDisplay { get; set; }


        //builds the address with out spaces
        public virtual List<string> populatedLines
        {
            get
            {
                List<string> outputAddress = new List<string>();
                if (AddressLine1 != null) outputAddress.Add(AddressLine1);
                if (AddressLine2 != null) outputAddress.Add(AddressLine2);
                if (AddressLine3 != null) outputAddress.Add(AddressLine3);
                if (Town != null) outputAddress.Add(Town);
                if (County != null) outputAddress.Add(County);
                if (Postcode != null) outputAddress.Add(Postcode);

                return outputAddress;
            }
        }

        [Display(Name = "Full address")]
        public virtual string DisplaySingleLineAddress
        {
            get
            {
                List<string> PopulateLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    PopulateLines.Add(SecurityElement.Escape(line));
                }
                if (populatedLines.Count > 0)
                {
                    return string.Join(", ", PopulateLines.ToArray());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
