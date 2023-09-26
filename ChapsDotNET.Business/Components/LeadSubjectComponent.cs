using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class LeadSubjectComponent : ILeadSubjectComponent
    {
        private readonly DataContext _context;

        public LeadSubjectComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of only active LeadSubjects
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of LeadSubjectModel</returns>
        public async Task<PagedResult<List<LeadSubjectModel>>> GetLeadSubjectsAsync(LeadSubjectRequestModel request)
        {
            var query = _context.LeadSubjects.AsQueryable();

            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Detail);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            if (!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                .Take(request.PageSize);
            }

            var leadSubjectsList = await query
                .Select(x => new LeadSubjectModel
                {
                    LeadSubjectId = x.LeadSubjectId,
                    Detail = x.Detail,
                    Active = x.active
                }).ToListAsync();

            return new PagedResult<List<LeadSubjectModel>>
            {
                Results = leadSubjectsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        public async Task<LeadSubjectModel> GetLeadSubjectAsync(int id)
        {
            var query = _context.LeadSubjects.AsQueryable();
            query = query.Where(x => x.LeadSubjectId == id);

            var leadSubject = await query
                .Select(x => new LeadSubjectModel
                {
                    LeadSubjectId = x.LeadSubjectId,
                    Detail = x.Detail,
                    Active = x.active
                }).SingleOrDefaultAsync();

            if (leadSubject == null)
            {
                return new LeadSubjectModel
                {
                    Detail = null
                };
            }
            return leadSubject;
        }

        public async Task<int> AddLeadSubjectAsync(LeadSubjectModel model)
        {
            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var leadSubject = new LeadSubject
            {
                active = true,
                Detail = model.Detail
            };

            await _context.LeadSubjects.AddAsync(leadSubject);
            await _context.SaveChangesAsync();
            return leadSubject.LeadSubjectId;
        }

        public async Task UpdateLeadSubjectAsync(LeadSubjectModel model)
        {
            var leadSubject = await _context.LeadSubjects.FirstOrDefaultAsync(x => x.LeadSubjectId == model.LeadSubjectId);

            if (leadSubject == null)
            {
                throw new NotFoundException("LeadSubject", model.LeadSubjectId.ToString());
            }

            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Detail", "cannot be empty");
            }

            leadSubject.active = model.Active;
            leadSubject.Detail = model.Detail;
            await _context.SaveChangesAsync();
        }
    }
}
