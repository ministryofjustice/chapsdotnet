using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChapsDotNET.DAL;

namespace ChapsDotNET.Entities;

public class CorrespondenceTypesByTeam
{
    [Key, Column(Order = 0)]
    public int TeamID { get; set; }
    [Key, Column(Order = 1)]
    public int CorrespondenceTypeID { get; set; }
    public virtual Team? Team { get; set; }
    public virtual CorrespondenceType? CorrespondenceType { get; set; }

}