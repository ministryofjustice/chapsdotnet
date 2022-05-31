using ChapsDotNET.DAL;

namespace ChapsDotNET.Entities;

public class User
{
    
    public int UserID { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string DisplayName { get; set; } = string.Empty;
    //[AdditionalMetadata("IgnoreAudit", true)]
    public DateTime? LastActive { get; set; }
    
    public int RoleStrength { get; set; }
    public virtual Role? Role { get; set; }
    
    public int TeamID { get; set; }
    public virtual Team? Team { get; set; }
    //[MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
    public string? email { get; set; }

    public bool Changeable { get; set; }

    public bool Auditable()
    {
        return true;
    }

}