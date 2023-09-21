using System;
using ChapsDotNET.Business.Interfaces;
using System.Linq;
using ChapsDotNET.Business.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Business.Components
{
	public class DisplayAlertsViewComponent : ViewComponent
	{
		private readonly IAlertComponent _alertComponent;

		public DisplayAlertsViewComponent(IAlertComponent alertComponent)
		{
			_alertComponent = alertComponent;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var alerts = await _alertComponent.GetCurrentAlertsAsync();

			if(alerts == null || !alerts.Any())
			{
				return Content(string.Empty);
			}	

			return View(alerts);
		}
	}
}

