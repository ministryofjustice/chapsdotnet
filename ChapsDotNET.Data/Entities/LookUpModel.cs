namespace ChapsDotNET.Data.Entities;

public abstract class LookUpModel
{

    public bool active { get; set; }
    public DateTime? deactivated { get; set; }
    public string deactivatedBy { get; set; } = string.Empty;
}