using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class PublicHolidayComponent : IPublicHolidayComponent
    {
        private readonly DataContext _context;

        public PublicHolidayComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of only active Public Holidays
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of PublicHoliday</returns>
        public async Task<PagedResult<List<PublicHolidayModel>>> GetPublicHolidaysAsync(PublicHolidayRequestModel request)
        {
            var query = _context.PublicHolidays.AsQueryable();

            query = query.OrderByDescending(x => x.Date);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                .Take(request.PageSize);

            var holidaysList = await query
                .Select(x => new PublicHolidayModel
                {
                    PublicHolidayID = x.PublicHolidayID,
                    Date = x.Date,
                    Description = x.Description
                }).ToListAsync();

            return new PagedResult<List<PublicHolidayModel>>
            {
                Results = holidaysList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                PageCount = count / request.PageSize,
                RowCount = count
            };
        }

        public async Task<PublicHolidayModel> GetPublicHolidayAsync(int id)
        {
            var query = _context.PublicHolidays.AsQueryable();
            query = query.Where(x => x.PublicHolidayID == id);

            var publicHoliday = await query
                .Select(x => new PublicHolidayModel
                {
                    PublicHolidayID = x.PublicHolidayID,
                    Description = x.Description,
                    Date = x.Date
                }).SingleOrDefaultAsync();

            if (publicHoliday == null)
            {
                return new PublicHolidayModel
                {
                    Description = null
                };
            }
            return publicHoliday;
        }


        public async Task<int> AddPublicHolidayAsync(PublicHolidayModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException(nameof(model.Description),"cannot be empty");
            }

            if (model.Date <= DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(model.Date), "cannot be in the past");
            }

            var publicHoliday = new PublicHoliday
            {
                Date = model.Date,
                Description = model.Description,
                PublicHolidayID = model.PublicHolidayID
            };

            await _context.PublicHolidays.AddAsync(publicHoliday);
            await _context.SaveChangesAsync();
            return publicHoliday.PublicHolidayID;
        }

        
        public async Task UpdatePublicHolidayAsync(PublicHolidayModel model)
        {
            var publicHoliday = await _context.PublicHolidays.FirstOrDefaultAsync(x => x.PublicHolidayID == model.PublicHolidayID);

            if (publicHoliday == null)
            {
                throw new NotFoundException("Public Holiday", model.PublicHolidayID.ToString());
            }

            if (publicHoliday.Date == DateTime.MinValue )
            {
                throw new ArgumentNullException(nameof(model.Description), "Parameter Date cannot be at the begining of epoc");
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException(nameof(model.Description), "Parameter Description cannot be empty");
            }

            publicHoliday.Date = model.Date;
            publicHoliday.Description = model.Description;

            await _context.SaveChangesAsync();
        }
    }
}
