namespace ChapsDotNET.Data.Entities;

public class Role
{

    public int strength { get; set; }
    public string Detail { get; set; } = string.Empty;
    public virtual ICollection<User>? Users { get; set; }

}

