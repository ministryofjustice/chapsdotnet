using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class EmailTemplate : IAuditable
    {
        public bool Auditable() { return true; }
        public int EmailTemplateID { get; set; }
        public int? CorrespondenceTypeID { get; set; }
        public int? StageID { get; set; }
        public bool Chaser { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string BodyText { get; set; } = string.Empty;
    }
}