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
    }
}