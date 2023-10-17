using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Http.Features;
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
        /// This method by default returns a list of Alerts
        /// </summary>
        /// <param name="request">AlertRequestModel</param>
        /// <returns>A list of Alert Models</returns>
        public async Task<List<AlertModel>> GetAlertsAsync(AlertRequestModel request)
		{
			var query = _context.Alerts.AsQueryable();

			if(!request.ShowActiveAndInactive)
			{
				query = query.Where(x => x.Live == true);
			}

			query = query.OrderByDescending(x => x.EventStart);

			if(!request.NoPaging)
			{
				query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
					.Take(request.PageSize);
			}

			var alertsList = await query.Select(x => new AlertModel
			{
				AlertID = x.AlertID,
				Live = x.Live,
				EventStart = x.EventStart,
				RaisedHours = x.RaisedHours,
				WarnStart = x.WarnStart,
				Message = x.Message ?? string.Empty
            }).ToListAsync();

			return alertsList;

		}

        /// <summary>
        /// This method by default returns an Alerts by ID
        /// </summary>
        /// <param name="id">Int id of the Alert</param>
        /// <returns>An Alert Model</returns>
        public async Task<AlertModel> GetAlertAsync(int id)
		{
			var query = _context.Alerts.AsQueryable();
			query = query.Where(x => x.AlertID == id);

			var alert = await query.Select(x => new AlertModel
			{
				AlertID = x.AlertID,
				Live = x.Live,
				EventStart = x.EventStart,
				RaisedHours = x.RaisedHours,
				WarnStart = x.WarnStart,
				Message = x.Message
			}).SingleOrDefaultAsync();

			if(alert == null)
			{
				return new AlertModel
				{
					Message = null,

				};
			}

			return alert;
		}


        /// <summary>
        /// This method by default returns a list of Alerts
        /// </summary>
        /// <param name="model">AlertModel with updated properties</param>
        public async Task UpdateAlertAsync(AlertModel model)
		{
			var alert = await _context.Alerts.FirstOrDefaultAsync(x => x.AlertID == model.AlertID)
				?? throw new NotFoundException("Alert", model.AlertID.ToString());

			if (string.IsNullOrEmpty(model.Message))
			{
				throw new ArgumentNullException("Parameter Message cannot be empty");
			}

			alert.AlertID = model.AlertID;
			alert.Live = model.Live;
			alert.EventStart = model.EventStart;
			alert.RaisedHours = model.RaisedHours;
			alert.WarnStart = model.WarnStart;
			alert.Message = model.Message;

			await _context.SaveChangesAsync();
        }

        /// <summary>
        /// This method by default adds an Alert
		/// </summary>
        /// <param name="model"></param>
        public async Task<int> AddAlertAsync(AlertModel model)
		{
            if (string.IsNullOrEmpty(model.Message))
            {
                throw new ArgumentNullException("Parameter Message cannot be empty");
            }

			var alert = new Alert
			{
				Live = model.Live,
				EventStart = model.EventStart,
				RaisedHours = model.RaisedHours,
				WarnStart = model.WarnStart,
				Message = model.Message
			};

			await _context.Alerts.AddAsync(alert);
			await _context.SaveChangesAsync();
			return alert.AlertID;
        }


        /// <summary>
        /// This method by default gets a list of current Alerts
        /// </summary>
        public async Task<List<AlertModel>> GetCurrentAlertsAsync()
		{
            var query = _context.Alerts.AsQueryable();
            query = query.Where(x => x.Live && x.WarnStart <= DateTime.Now);

			var alertsList = await query.Select(x => new AlertModel
            {
                AlertID = x.AlertID,
                Live = x.Live,
                EventStart = x.EventStart,
                RaisedHours = x.RaisedHours,
                WarnStart = x.WarnStart,
                Message = x.Message ?? string.Empty
            }).ToListAsync();

			return alertsList;
		}
    }
}

