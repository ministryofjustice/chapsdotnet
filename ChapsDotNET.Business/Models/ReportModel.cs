namespace ChapsDotNET.Business.Models
{
    public class ReportModel
    {
        public int ReportId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
    }
}

