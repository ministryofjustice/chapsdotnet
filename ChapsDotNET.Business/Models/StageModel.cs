

namespace ChapsDotNET.Business.Models
{
    public class StageModel
    {
        public int StageID { get; set; }
        public required string Name { get; set; }
        public int WorkingDayLimit { get; set; }
        public bool ChangeableTarget { get; set; }
        public int LinkedStatus { get; set; }
        public int? CanBeAssignedToTeamID { get; set; }
        public int OversightByTeamID { get; set; }
        public bool RequiresEmailUpdate { get; set; }
        public TimeSpan TimeDue { get; set; }
        public bool NoAssignmentRequired { get; set; }
        // TODO: public virtual ICollection<wfPath> AvailablePaths { get; set; }
        //public virtual Team? CanBeAssignedToTeam { get; set; }
        //public virtual Team? OversightByTeam { get; set; }
        public int? TrackingID { get; set; } = 0;
    }
}
