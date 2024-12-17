using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
	public class Alert : IAuditable
	{
		public int AlertID { get; set; }
        public bool Live { get; set; }
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

