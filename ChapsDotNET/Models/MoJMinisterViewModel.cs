using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class MoJMinisterViewModel
    {
        public int MoJMinisterId { get; set; }

        [Required, MaxLength(50), Display(Name = "Title")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Prefix { get; set; }

        [MaxLength(20)]
        public string? Suffix { get; set; }

        [MaxLength(150), Display(Name = "Title")]
        public string? Rank { get; set; } 

        public bool Active { get; set; }
    }
}
