using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class Alert : LookUpModel, IAuditable
    {
        public int AlertId { get; set; }
        public bool live { get; set; }
        public DateTime EventStart { get; set; }
        public int RaisedHours { get; set; }
        public DateTime WarnStart { get; set; }
        public string? Message { get; set; }

        public bool Auditable()
        {
            return true;
        }
    }
}