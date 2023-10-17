namespace ChapsDotNET.Business.Models
{
    public class CampaignModel
    {
        public int CampaignId { get; set; }
        public string Detail { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime? Deactivated { get; set; }
        public string? DeactivatedBy { get; set; }
    }
}
