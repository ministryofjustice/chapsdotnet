using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class MoJMinisterViewModel
    {
        [Key]
        public int MoJMinisterId { get; set; }

        [Required, MaxLength(50), Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(30), Display(Name="Prefix")]
        public string? Prefix { get; set; }

        [MaxLength(20), Display(Name="Suffix")]
        public string? Suffix { get; set; }

        [MaxLength(150), Display(Name = "Position")]
        public string? Rank { get; set; } 

        public bool Active { get; set; }
    }
}
