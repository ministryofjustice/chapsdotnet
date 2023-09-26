using ChapsDotNET.Data.Entities;
using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Business.Models
{
    public class LeadSubjectModel : LookUpModel, IAuditable
    {
        public int LeadSubjectId { get; set; }
        public string? Detail { get; set; }
        public bool Active { get; set; }

        public bool Auditable()
        {
            return true;
        }
    }
}

