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

        public async Task<List<SalutationModel>> GetAllSalutationsAsync()
        {
            var salutationsList = await _context.Salutations.Select(x => new SalutationModel
            {
                SalutationId = x.salutationID,
                Detail = x.Detail
            }).ToListAsync();

            return salutationsList;
        }
    }
}

