namespace ChapsDotNET.Business.Models
{
    public class MoJMinisterModel
    {
        public int MoJMinisterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? Rank { get; set; } 
        public bool Active { get; set; }
        public DateTime? Deactivated { get; set; }
        public string? DeactivatedBy { get; set; }
    }   
}
