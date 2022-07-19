using ChapsDotNET.Data.Interfaces;

namespace ChapsDotNET.Data.Entities;

public class Team : LookUpModel, IAuditable
{
    public int TeamID { get; set; }
    public string Acronym { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    //Commented out because we will be using this formatting to define the email.
    //[MaxLength(80), RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
    public string? email { get; set; }
    public bool isOGD { get; set; }
    public bool isPOD { get; set; }
    public virtual ICollection<CorrespondenceTypesByTeam>? CorrespondenceTypes { get; set; }
    public virtual ICollection<User>? Users { get; set; }

    public Team()
    {
        isOGD = false;
    }

    public bool Auditable()
    {
        return true;
    }
}
