using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities
{
	public class MoJMinister : LookUpModel, IAuditable
	{
        public int MoJMinisterID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? prefix { get; set; }
        public string? suffix { get; set; }
        public string? Rank { get; set; } 
        public bool Auditable()
        {
            return true;
        }
	}
}
