using ChapsDotNET.Business.Models;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ITeamComponent
	{
		public Task<TeamModel> GetUserTeamAsync(int Id);
	}
}
