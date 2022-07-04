namespace ChapsDotNET.Business.Models
{
	public class TeamModel
	{
		public int TeamID { get; set; }
		public string? Acronym { get; set; }
		public bool Active { get; set; }
		public string email { get; set; }  = string.Empty;
		public string? Name { get; set; }
		public bool	isOGD { get; set; }
		public bool isPOD { get; set; }
	}
}
