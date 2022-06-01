namespace ChapsDotNET.Data.Entities;

public class CorrespondenceType : LookUpModel
{

    public int CorrespondenceTypeID { get; set; }
    public string Acronym { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool LimitedOnly { get; set; }
    public int? TargetDays { get; set; }
    public virtual ICollection<CorrespondenceTypesByTeam>? Teams { get; set; }
    public bool Signatory { get; set; }
}