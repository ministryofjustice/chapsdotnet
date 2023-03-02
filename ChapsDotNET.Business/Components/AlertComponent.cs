using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class AlertComponent : IAlertComponent
    {
        private readonly DataContext _context;

        public AlertComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of only active Alerts
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of AlertModel</returns>
        public async Task<PagedResult<List<AlertModel>>> GetAlertsAsync(AlertRequestModel request)
        {
            var query = _context.Alerts.AsQueryable();

            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Message);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                .Take(request.PageSize);

            var AlertsList = await query
                .Select(x => new AlertModel
                {
                    AlertId = x.AlertId,
                    Message = x.Message,
                    Live = x.live
                }).ToListAsync();

            return new PagedResult<List<AlertModel>>
            {
                Results = AlertsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        public async Task<AlertModel> GetAlertAsync(int id)
        {
            var query = _context.Alerts.AsQueryable();
            query = query.Where(x => x.AlertId == id);

            var Alert = await query
                .Select(x => new AlertModel
                {
                    AlertId = x.AlertId,
                    Message = x.Message,
                    Live = x.live
                }).SingleOrDefaultAsync();

            if (Alert == null)
            {
                return new AlertModel
                {
                    Message = null
                };
            }
            return Alert;
        }

        public async Task<int> AddAlertAsync(AlertModel model)
        {
            if (string.IsNullOrEmpty(model.Message))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var Alert = new Alert
            {
                active = true,
                Message = model.Message
            };

            await _context.Alerts.AddAsync(Alert);
            await _context.SaveChangesAsync();
            return Alert.AlertId;
        }

        public async Task UpdateAlertAsync(AlertModel model)
        {
            var Alert = await _context.Alerts.FirstOrDefaultAsync(x => x.AlertId == model.AlertId);

            if (Alert == null)
            {
                throw new NotFoundException("Alert", model.AlertId.ToString());
            }

            if (string.IsNullOrEmpty(model.Message))
            {
                throw new ArgumentNullException("Message", "cannot be empty");
            }

            Alert.live = model.Live;
            Alert.Message = model.Message;
            await _context.SaveChangesAsync();
        }
    }
}
