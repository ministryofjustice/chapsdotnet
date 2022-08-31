using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class MPComponent : IMPComponent
    {
        private readonly DataContext _context;

        public MPComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of only active MPs
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of MPModel</returns>
        public async Task<PagedResult<List<MPModel>>> GetMPAsync(MPRequestModel request)
        {
            var query = _context.MPs.AsQueryable();

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

            var mpsList = await query
                .Select(x => new MPModel
                {
                    MPID = x.mpID,
                    Detail = x.Detail,
                    Active = x.active
                }).ToListAsync();

            return new PagedResult<List<MPModel>>
            {
                Results = mpsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                PageCount = count / request.PageSize,
                RowCount = count
            };
        }

        public async Task<MPModel> GetMPAsync(int id)
        {
            var query = _context.MPs.AsQueryable();
            query = query.Where(x => x.mpID == id);

            var mp = await query
                .Select(x => new MPModel
                {
                    MPID = x.mpID,
                    Detail = x.Detail,
                    Active = x.active
                }).SingleOrDefaultAsync();

            if (mp == null)
            {
                return new MPModel
                {
                    Detail = null
                };
            }
            return mp;
        }

        public async Task<int> AddMPAsync(MPModel model)
        {
            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var mp = new MP
            {
                active = true,
                Detail = model.Detail
            };

            await _context.MPs.AddAsync(mp);
            await _context.SaveChangesAsync();
            return mp.mpID;
        }

        public async Task UpdateMPsAsync(MPModel model)
        {
            var mp = await _context.MPs.FirstOrDefaultAsync(x => x.mpID == model.MPID);

            if (mp == null)
            {
                throw new NotFoundException("mp", model.MPID.ToString());
            }

            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            mp.active = model.Active;
            mp.Detail = model.Detail;
            await _context.SaveChangesAsync();
        }

        
    }
}
