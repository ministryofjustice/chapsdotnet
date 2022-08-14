using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class PublicHoliday : IAuditable
    {
        public int PublicHolidayID { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; } 

        public bool Auditable()
        {
            return true;
        }
    }
}

