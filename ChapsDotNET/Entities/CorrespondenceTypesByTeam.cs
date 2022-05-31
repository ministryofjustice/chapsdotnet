using ChapsDotNET.DAL;

namespace ChapsDotNET.Entities;

public class CorrespondenceTypesByTeam
{
    
    public int TeamID { get; set; }
    public int CorrespondenceTypeID { get; set; }
    public virtual Team? Team { get; set; }
    public virtual CorrespondenceType? CorrespondenceType { get; set; }

}