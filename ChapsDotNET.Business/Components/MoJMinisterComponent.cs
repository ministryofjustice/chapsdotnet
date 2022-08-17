using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class MoJMinisterComponent : IMoJMinisterComponent
    {
        private readonly DataContext _context;

        public MoJMinisterComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of only active MoJ Ministers
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of MoJ Minister Model</returns>
        public async Task<PagedResult<List<MoJMinisterModel>>> GetMoJMinistersAsync(MoJMinisterRequestModel request)
        {
            var query = _context.MoJMinisters.AsQueryable();

            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Name);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                .Take(request.PageSize);

            var MoJMinistersList = await query
                .Select(x => new MoJMinisterModel
                {
                    MoJMinisterId = x.MoJMinisterID,
                    Name = x.Name,
                    Prefix = x.prefix,
                    Suffix = x.suffix,
                    Rank = x.Rank,
                    Active = x.active
                }).ToListAsync();

            return new PagedResult<List<MoJMinisterModel>>
            {
                Results = MoJMinistersList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                PageCount = count / request.PageSize,
                RowCount = count
            };
        }

        //public async Task<MoJMinisterModel> GetMoJMinisterAsync(int id)
        //{
        //    var query = _context.MoJMinisters.AsQueryable();
        //    query = query.Where(x => x.salutationID == id);

        //    var salutation = await query
        //        .Select(x => new MoJMinisterModel
        //        {
        //            MoJMinisterId = x.salutationID,
        //            Detail = x.Detail,
        //            Active = x.active
        //        }).SingleOrDefaultAsync();

        //    if (salutation == null)
        //    {
        //        return new MoJMinisterModel
        //        {
        //            Detail = null
        //        };
        //    }
        //    return salutation;
        //}

        //public async Task<int> AddMoJMinisterAsync(MoJMinisterModel model)
        //{
        //    if (string.IsNullOrEmpty(model.Detail))
        //    {
        //        throw new ArgumentNullException("Parameter Detail cannot be empty");
        //    }

        //    var salutation = new MoJMinister
        //    {
        //        active = true,
        //        Detail = model.Detail
        //    };

        //    await _context.MoJMinisters.AddAsync(salutation);
        //    await _context.SaveChangesAsync();
        //    return salutation.salutationID;
        //}

        //public async Task UpdateMoJMinisterAsync(MoJMinisterModel model)
        //{
        //    var salutation = await _context.MoJMinisters.FirstOrDefaultAsync(x => x.salutationID == model.MoJMinisterId);

        //    if (salutation == null)
        //    {
        //        throw new NotFoundException("MoJMinister", model.MoJMinisterId.ToString());
        //    }

        //    if (string.IsNullOrEmpty(model.Detail))
        //    {
        //        throw new ArgumentNullException("Parameter Detail cannot be empty");
        //    }

        //    salutation.active = model.Active;
        //    salutation.Detail = model.Detail;
        //    await _context.SaveChangesAsync();
        //}
    }
}
