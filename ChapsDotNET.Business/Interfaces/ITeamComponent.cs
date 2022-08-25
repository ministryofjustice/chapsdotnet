using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ITeamComponent
    {
        Task<PagedResult<List<TeamModel>>> GetTeamsAsync(TeamRequestModel request);
        Task<int> AddTeamAsync(TeamModel model);
        Task UpdateTeamAsync(TeamModel model);
        Task<TeamModel> GetTeamAsync(int id);
    }
}
