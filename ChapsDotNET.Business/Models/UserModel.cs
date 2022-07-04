namespace ChapsDotNET.Business.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public int RoleStrength { get; set; }
        public int TeamId { get; set; }
        public string? TeamAcronym { get; set; }
    }
}
