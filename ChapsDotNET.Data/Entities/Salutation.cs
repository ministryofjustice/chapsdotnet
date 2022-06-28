using ChapsDotNET.Data.Interfaces;


namespace ChapsDotNET.Data.Entities
{
    public class Salutation : LookUpModel, IAuditable
    {
        public int salutationID { get; set; }

        public string? Detail { get; set; }

        public bool Auditable()
        {
            return true;
        }


    }
}
