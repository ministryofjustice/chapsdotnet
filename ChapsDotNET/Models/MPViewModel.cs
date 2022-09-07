using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Models
{
    public class MPViewModel
    {
        [Key]
        public int MPId { get; set; }

        public int SalutationId { get; set; }

        [Required, MaxLength(50)]
        public string Surname { get; set; } = String.Empty;

        [MaxLength(50)]
        public string? FirstNames { get; set; }

        [MaxLength(100)]
        public string? AddressLine1 { get; set; }

        [MaxLength(100)]
        public string? AddressLine2 { get; set; }

        [MaxLength(100)]
        public string? AddressLine3 { get; set; }

        [MaxLength(100)]
        public string? Town { get; set; }

        [MaxLength(100)]
        public string? County { get; set; }

        [MaxLength(10)]
        public string? Postcode { get; set; }

        [MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }

        [Required]
        public bool RtHon { get; set; }

        [MaxLength(20)]
        public string? Suffix { get; set; }

        public bool Active { get; set; }

        //[Display(Name = "Name")]
        //public virtual string DisplayName
        //{
        //    get
        //    {
        //        List<string> nameParts = new List<string>();
        //        nameParts.Add(RtHon);
        //        nameParts.Add(SalutationId.ToString() != null ? Salutation.Details : null);
        //        nameParts.Add(FirstNames != null ? FirstNames : null);
        //        nameParts.Add(Surname);
        //        nameParts.Add(Suffix != null ? Suffix : null);
        //        return string.Join(" ", nameParts.Where(s => !string.IsNullOrEmpty(s)));
        //    }
        //}
    }
}
