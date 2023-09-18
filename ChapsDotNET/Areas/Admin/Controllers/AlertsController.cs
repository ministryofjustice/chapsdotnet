using System;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ChapsDotNET.Areas.Admin.Controllers
{
	public class AlertsController : Controller
	{
		private readonly IAlertComponent _alertComponent;

		public AlertsController(IAlertComponent alertComponent)
		{
			_alertComponent = alertComponent;
		}

		public async Task<IActionResult> Index(int page = 1)
		{
			var pagedResult = await _alertComponent.GetAlertsAsync(new AlertRequestModel
			{
				PageNumber = page,
				PageSize = 10,
				ShowActiveAndInactive = true
			});

			return View(pagedResult);

		}

		public ActionResult Create()
		{
			var model = new AlertViewModel();
			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var model = await _alertComponent.GetAlertAsync(id);
			return View(model.ToViewModel());
		}

		public async Task<ActionResult> Edit(AlertViewModel viewmodel)
		{
			await _alertComponent.UpdateAlertAsync(viewmodel.ToModel());
			return RedirectToAction("Index");
		}
	}


}
