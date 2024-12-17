using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class SalutationComponent(DataContext context) : ISalutationComponent
    {
        //private readonly IHttpContextAccessor? _httpContextAccessor;

        /// <summary>
        /// Returns a list of active Salutations
        /// </summary>
        /// <param name="request">SalutationRequestModel</param>
        /// <returns>A paged list of SalutationModels</returns>
        public async Task<PagedResult<List<SalutationModel>>> GetSalutationsAsync(SalutationRequestModel request)
        {
            var query = context.Salutations.AsQueryable();

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

        /// <summary>
        /// Returns a single SalutationModel
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>A SalutationModel</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<SalutationModel> GetSalutationAsync(int id)
        {
            var salutation = await context.Salutations
                .FirstOrDefaultAsync(x => x.salutationID == id);

            if (salutation == null) throw new NotFoundException("Salutation", id.ToString());

            return new SalutationModel
            {
                SalutationId = salutation.salutationID,
                Detail = salutation.Detail,
                Active = salutation.active
            };
        }

        /// <summary>
        /// Adds a salutation
        /// </summary>
        /// <param name="model">SalutationModel</param>
        /// <returns>Int SalutationId</returns>
        /// <exception cref="ArgumentNullException"></exception>
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

            await context.Salutations.AddAsync(salutation);
            await context.SaveChangesAsync();

            return salutation.salutationID;
        }

        /// <summary>
        /// Updates a Salutation
        /// </summary>
        /// <param name="model">SalutationModel</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateSalutationAsync(SalutationModel model)
        {
            var salutation = await context.Salutations.FirstOrDefaultAsync(x => x.salutationID == model.SalutationId);

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
            await context.SaveChangesAsync();
        }
    }
}
