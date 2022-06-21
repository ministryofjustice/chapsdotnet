using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
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
        /// <param name="showActiveAndInactive">If this parameter is true, then you get a list of active and inactive salutations</param>
        /// <returns>A list of SalutationModel</returns>
        public async Task<List<SalutationModel>> GetSalutationsAsync(bool showActiveAndInactive = false)
        {
            var query = _context.Salutations.AsQueryable();
            
            if (!showActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            var salutationsList = await query
                .Select(x => new SalutationModel
                {
                    SalutationId = x.salutationID,
                    Detail = x.Detail,
                    Active = x.active
                }).ToListAsync();

            return salutationsList;
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

