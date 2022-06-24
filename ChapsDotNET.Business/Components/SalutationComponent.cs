using System.Reflection.Metadata.Ecma335;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using ChapsDotNET.Data.Entities;

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
                PageCount = count / request.PageSize,
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
                }).SingleOrDefaultAsync();

            return salutation;     
        }

        public async Task<string> AddSalutation(SalutationModel model)
        {
            var context = _context;
            Salutation sal = new Salutation()
            {
                active = model.Active,
                Detail = model.Detail
            };
            context.Salutations.Add(sal);
            var success = await context.SaveChangesAsync();
            return success > 0 ? "Success" : "Fail";
        }



        public void UpdateSalutationActiveStatus(int id, bool state)
        {
            var context = _context;

            var query = _context.Salutations.AsQueryable();
            query = query.Where(x => x.salutationID == id);

            foreach(var q in query)
            {
                q.active = false;
            };




            //var salutation = query
            //.Select(x => new SalutationModel
            //{
            //    SalutationId = x.salutationID,
            //    Detail = x.Detail,
            //    Active = state
            //}).SingleAsync();

            context.SaveChanges();
        }
    }
}

