using ChapsDotNET.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapsDotNET.Data.Entities
{
    public class Stage
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
        public virtual Team? CanBeAssignedToTeam { get; set; }
        public virtual Team? OversightByTeam { get; set; }
        public int? TrackingID { get; set; }
    }
}
