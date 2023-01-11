using System.ComponentModel.DataAnnotations;
using System.Security;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Models
{
    public class MPViewModel
    {
        [Key]
        public int MPId { get; set; }

        public int? SalutationId { get; set; }
        public Salutation Salutation { get; set; } = default!;

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
        public SelectList SalutationList { get; set; } = default!;
        public string? DisplayFullName { get; set; }
        public string? SortOrder { get; set; }
    }
}
