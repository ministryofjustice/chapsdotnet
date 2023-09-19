using System;
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
				//RaisedHours = model.RaisedHours,- not sure how this is used yet. Probably not input by the user though
				WarnStart = model.WarnStart,
				Message = model.Message
			};

			await _context.Alerts.AddAsync(alert);
			await _context.SaveChangesAsync();
			return alert.AlertID;
        }
    }
}

