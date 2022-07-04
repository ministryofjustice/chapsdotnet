using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class TeamComponent : ITeamComponent
	{
        private readonly DataContext _context;

        public TeamComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<TeamModel> GetUserTeamAsync(int userTeamId)
        {
            var query = _context.Teams.AsQueryable();

            query = query.Where(x => x.TeamID == userTeamId);

            var userTeam = await query
                .Select(x => new TeamModel
                {
                    TeamID = x.TeamID,
                    Acronym = x.Acronym,
                    Active = x.active,
		            email = x.email,
		            Name = x.Name,
		            isOGD = x.isOGD,
		            isPOD = x.isPOD
                }).SingleOrDefaultAsync();

            if (userTeam == null)
            {
                return new TeamModel
                {
                    Acronym = null,
                    Name = null
                };
            }
            return userTeam;
        }
    }
}
