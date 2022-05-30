using System.ComponentModel.DataAnnotations;
using ChapsDotNET.DAL;

namespace ChapsDotNET.Entities;

public class CorrespondenceType : LookUpModel
{
    [Key]
    public int CorrespondenceTypeID { get; set; }

    [Required, MaxLength(3)]
    public string Acronym { get; set; } = string.Empty;
    [Required, MaxLength(30)]
    public string Name { get; set; } = string.Empty;
    public bool LimitedOnly { get; set; }
    public int? TargetDays { get; set; }
    public virtual ICollection<CorrespondenceTypesByTeam>? Teams { get; set; }
    public bool Signatory { get; set; }
}