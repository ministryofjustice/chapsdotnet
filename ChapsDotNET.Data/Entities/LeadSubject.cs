using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class LeadSubject : LookUpModel, IAuditable
    {
        public int LeadSubjectID { get; set; }

        public string? Detail { get; set; }

        public bool Auditable()
        {
            return true;
        }
    }
}