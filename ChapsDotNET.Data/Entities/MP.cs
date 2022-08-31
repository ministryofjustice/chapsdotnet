using ChapsDotNET.Data.Interfaces;


namespace ChapsDotNET.Data.Entities
{
    public class MP : LookUpModel, IAuditable
    {
        public int mpID { get; set; }

        public string? Detail { get; set; }

        public bool Auditable()
        {
            return true;
        }


    }
}
