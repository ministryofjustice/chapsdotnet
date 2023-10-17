using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
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

        /// <summary>
        /// Returns a list of Teams
        /// </summary>
        /// <param name="request">TeamRequestModel</param>
        /// <returns>A list of TeamModels</returns>
        public async Task<PagedResult<List<TeamModel>>> GetTeamsAsync(TeamRequestModel request)
        {
            var query = _context.Teams.AsQueryable();

            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Name);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            if (!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var teamsList = await query
                .Select(x => new TeamModel
                {
                    TeamId = x.TeamID,
                    Name = x.Name,
                    Acronym = x.Acronym,
                    Active = x.active,
                    Email = x.email,
                    IsOgd = x.isOGD,
                    IsPod = x.isPOD,
                    deactivated = x.deactivated,
                    deactivatedBy = x.deactivatedBy
                }).ToListAsync();

            return new PagedResult<List<TeamModel>>
            {
                Results = teamsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        /// <summary>
        /// Returns a single TeamModel
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>TeamModel</returns>
        public async Task<TeamModel> GetTeamAsync(int id)
        {
            var query = _context.Teams.AsQueryable();
            query = query.Where(x => x.TeamID == id);

            var team = await query
                .Select(x => new TeamModel
                {
                    TeamId = x.TeamID,
                    Name = x.Name,
                    Acronym = x.Acronym,
                    Active = x.active,
                    Email = x.email,
                    IsOgd = x.isOGD,
                    IsPod = x.isPOD,
                    deactivated = x.deactivated,
                    deactivatedBy = x.deactivatedBy
             
                }).SingleOrDefaultAsync();

            if (team == null)
            {
                return new TeamModel
                {
                    Name = null
                };
            }
            return team;
        }

        /// <summary>
        /// Adds a Team
        /// </summary>
        /// <param name="model">TeamModel</param>
        /// <returns>Int TeamID</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> AddTeamAsync(TeamModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var team = new Team
            {
                active = true,
                Name = model.Name,
                Acronym = model.Acronym,
                email = model.Email,
                isOGD = model.IsOgd,
                isPOD = model.IsPod
            };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return team.TeamID;
        }

        /// <summary>
        /// Updates a Team
        /// </summary>
        /// <param name="model">TeamModel</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateTeamAsync(TeamModel model)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamID == model.TeamId);

            if (team == null)
            {
                throw new NotFoundException("Team", model.TeamId.ToString());
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Parameter Name cannot be empty");
            }

            team.Acronym = model.Acronym;
            team.active = model.Active;
            team.Name = model.Name;
            team.email= model.Email;
            team.isOGD = model.IsOgd;
            team.isPOD = model.IsPod;
            team.deactivated = model.deactivated;
            team.deactivatedBy = model.deactivatedBy;

            //_context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }
    }
}

