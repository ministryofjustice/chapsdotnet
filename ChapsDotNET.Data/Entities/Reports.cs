using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class Report : IAuditable
    {
        public int ReportId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 

        public bool Auditable()
        {
            return true;
        }
    }
}
