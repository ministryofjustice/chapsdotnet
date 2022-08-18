using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
    public class Campaign : LookUpModel, IAuditable
    {
        public int CampaignID { get; set; }
        public string Detail { get; set; }  = string.Empty;
        public bool Auditable()
        {
            return true;
        }
    }
}
