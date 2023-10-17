using System;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
	public class RoleComponent : IRoleComponent
	{
        private readonly DataContext _context;

        public RoleComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Roles
        /// </summary>
        /// <param name="request">RoleRequestModel</param>
        /// <returns>A paged list of RoleModels</returns>
        public async Task<PagedResult<List<RoleModel>>> GetRolesAsync(RoleRequestModel request)
        {
            var query = _context.Roles.AsQueryable();
            query = query.OrderBy(x => x.strength);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            if (!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var rolesList = await query
                .Select(x => new RoleModel
                {
                    RoleStrength = x.strength,
                    Detail = x.Detail
                }).ToListAsync();

            return new PagedResult<List<RoleModel>>
            {
                Results = rolesList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }
    }
}

