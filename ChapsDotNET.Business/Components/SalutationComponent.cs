using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
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
                RowCount = count
            };
        }

        public async Task<SalutationModel> GetSalutationAsync(int id)
        {
            var salutation = await _context.Salutations
                .FirstOrDefaultAsync(x => x.salutationID == id);

            if (salutation == null) throw new NotFoundException("Salutation", id.ToString());

            return new SalutationModel
            {
                SalutationId = salutation.salutationID,
                Detail = salutation.Detail,
                Active = salutation.active
            };
        }

        public async Task<int> AddSalutationAsync(SalutationModel model)
        {
            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var salutation = new Salutation
            {
                active = true,
                Detail = model.Detail
            };
            await _context.Salutations.AddAsync(salutation);
            await _context.SaveChangesAsync();

            return salutation.salutationID;
        }



        public async Task UpdateSalutationAsync(SalutationModel model)
        {
            var salutation = await _context.Salutations.FirstOrDefaultAsync(x => x.salutationID == model.SalutationId);

            if (salutation == null)
            {
                throw new NotFoundException("Salutation", model.SalutationId.ToString());
            }

            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            salutation.active = model.Active;
            salutation.Detail = model.Detail;

            await _context.SaveChangesAsync();
        }
    }
}

