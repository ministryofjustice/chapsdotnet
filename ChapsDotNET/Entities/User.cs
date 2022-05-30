using System.ComponentModel.DataAnnotations;
using ChapsDotNET.DAL;

namespace ChapsDotNET.Entities;

public class User
{
    [Key]
    public int UserID { get; set; }
    [Required, MaxLength(150, ErrorMessage = "Login Name must be less than 151 characters long"), Display(Name = "Login Name")]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(100), Display(Name = "Display Name")]
    public string DisplayName { get; set; } = string.Empty;
    //[AdditionalMetadata("IgnoreAudit", true)]
    public DateTime? LastActive { get; set; }
    [Required(ErrorMessage = "A role is mandatory"), Display(Name = "Role")]
    public int RoleStrength { get; set; }
    public virtual Role? Role { get; set; }
    [Required(ErrorMessage = "An office is mandatory"), Display(Name = "Team")]
    public int TeamID { get; set; }
    public virtual Team? Team { get; set; }
    [MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
    public string email { get; set; } = string.Empty;
    public bool Changeable { get; set; }

    public bool Auditable()
    {
        return true;
    }

}