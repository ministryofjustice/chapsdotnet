namespace ChapsDotNET.Business.Models
{
    public class TeamModel
    {
        public int TeamId { get; set; }
        public string? Acronym { get; set; } 
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsOgd { get; set; }
        public bool IsPod { get; set; }
        public bool Active { get; set; }
    }
}
