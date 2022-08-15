using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Models
{
    public class CampaignViewModel
    {
        public int CampaignId { get; set; }
        [Required, MaxLength(80), Display(Name = "Description")]
        public string Detail { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
