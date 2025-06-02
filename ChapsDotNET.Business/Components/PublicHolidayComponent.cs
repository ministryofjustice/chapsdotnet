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
        /// Returns a list of Public Holidays
        /// </summary>
        /// <param name="request">PublicHolidayRequestModel</param>
        /// <returns>A list of PublicHolidayModels</returns>
        public async Task<PagedResult<List<PublicHolidayModel>>> GetPublicHolidaysAsync(PublicHolidayRequestModel request)
        {
            var query = _context.PublicHolidays.AsQueryable();

            query = query.OrderByDescending(x => x.Date);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            if (!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var holidaysList = await query
                .Select(x => new PublicHolidayModel
                {
                    PublicHolidayId = x.PublicHolidayID,
                    Date = x.Date,
                    Description = x.Description
                }).ToListAsync();

            return new PagedResult<List<PublicHolidayModel>>
            {
                Results = holidaysList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        /// <summary>
        /// Returns a single PublicHolidayModel
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>a single PublicHolidayModel</returns>
        public async Task<PublicHolidayModel> GetPublicHolidayAsync(int id)
        {
            var query = _context.PublicHolidays.AsQueryable();
            query = query.Where(x => x.PublicHolidayID == id);

            var publicHoliday = await query
                .Select(x => new PublicHolidayModel
                {
                    PublicHolidayId = x.PublicHolidayID,
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

        /// <summary>
        /// Adds a PublicHoliday
        /// </summary>
        /// <param name="model">PublicHolidayModel</param>
        /// <returns>Int</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<int> AddPublicHolidayAsync(PublicHolidayModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException(nameof(model.Description), "cannot be empty");
            }

            if (model.Description.Length > 30)
            {
                throw new ArgumentOutOfRangeException("Description", model.Description, "Cannot be longer then 30 characters");
            }

            if (model.Date <= DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(model.Date), "cannot be in the past");
            }

            var publicHoliday = new PublicHoliday
            {
                Date = model.Date,
                Description = model.Description,
                PublicHolidayID = model.PublicHolidayId
            };

            await _context.PublicHolidays.AddAsync(publicHoliday);
            await _context.SaveChangesAsync();
            return publicHoliday.PublicHolidayID;
        }

        /// <summary>
        /// Updates a PublicHoliday
        /// </summary>
        /// <param name="model">PublicHolidayModel</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<int> UpdatePublicHolidayAsync(PublicHolidayModel model)
        {
            var publicHoliday = await _context.PublicHolidays.FirstOrDefaultAsync(x => x.PublicHolidayID == model.PublicHolidayId);

            if (publicHoliday == null)
            {
                throw new NotFoundException("Public Holiday", model.PublicHolidayId.ToString());
            }

            if (publicHoliday.Date == DateTime.MinValue)
            {
                throw new ArgumentNullException(model.Description, "cannot be at the begining of epoch");
            }
            if (model.Date <= DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("Date", model.Date.ToShortDateString(), "Cannot be in the past");
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException(model.Description, "cannot be empty");
            }

            if (model.Description.Length > 30)
            {
                throw new ArgumentOutOfRangeException("Description", model.Description, "Cannot be longer then 30 characters");
            }

            publicHoliday.Date = model.Date;
            publicHoliday.Description = model.Description;

            await _context.SaveChangesAsync();
            return publicHoliday.PublicHolidayID;
        }
    }
}
