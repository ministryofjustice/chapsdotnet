using System.Reflection.Metadata.Ecma335;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class SalutationComponent : ISalutationComponent
    {
        private readonly DataContext _context;

        public SalutationComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of only active Salutations
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of SalutationModel</returns>
        public async Task<PagedResult<List<SalutationModel>>> GetSalutationsAsync(SalutationRequestModel request)
        {
            var query = _context.Salutations.AsQueryable();
            
            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Detail);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                .Take(request.PageSize);

            var salutationsList = await query
                .Select(x => new SalutationModel
                {
                    SalutationId = x.salutationID,
                    Detail = x.Detail,
                    Active = x.active
                }).ToListAsync();

            return new PagedResult<List<SalutationModel>>
            {
                Results = salutationsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                PageCount = salutationsList.Count / request.PageSize,
                RowCount = count
            };
        }

        public async Task<SalutationModel> GetSalutationAsync(int id)
        {
            var query = _context.Salutations.AsQueryable();
            query = query.Where(x => x.salutationID == id);

            var salutation = await query
                .Select(x => new SalutationModel
                {
                    SalutationId = x.salutationID,
                    Detail = x.Detail,
                    Active = x.active
                }).SingleAsync();

            return salutation;


        }
    }
}

