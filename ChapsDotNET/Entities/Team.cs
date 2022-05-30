using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Entities;
using ChapsDotNET.Interfaces;

namespace ChapsDotNET.DAL;

public class Team : LookUpModel, IAuditable
{
    [Key]
    public int TeamID { get; set; }
    [Required, MaxLength(10), Display(Name = "Team Acronym")]
    public string Acronym { get; set; } = string.Empty;
    [Required, MaxLength(40), Display(Name = "Team")]
    public string Name { get; set; } = string.Empty;
    [MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
    public string email { get; set; } = string.Empty;
    [Display(Name = "OGD?")]
    public bool isOGD { get; set; }
    [Display(Name = "Private Office?")]
    public bool isPOD { get; set; }
    public virtual ICollection<CorrespondenceTypesByTeam>? CorrespondenceTypes { get; set; }

    public Team()
    {
        isOGD = false;
    }

    public bool Auditable()
    {
        return true;
    }


}