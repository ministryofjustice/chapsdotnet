namespace ChapsDotNET.Data.Entities;

public class User
{
    public int UserID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime? LastActive { get; set; }
    public int RoleStrength { get; set; }
    public virtual Role? Role { get; set; }
    public int TeamID { get; set; }
    public virtual Team? Team { get; set; }
    public string? email { get; set; }
    public bool Changeable { get; set; }

    public bool Auditable()
    {
        return true;
    }
}
