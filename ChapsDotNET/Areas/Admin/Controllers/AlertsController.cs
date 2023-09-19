using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AlertsController : Controller
	{
		private readonly IAlertComponent _alertComponent;

		public AlertsController(IAlertComponent alertComponent)
		{
			_alertComponent = alertComponent;
		}

		public async Task<IActionResult> Index(int page = 1)
		{
			var alerts = await _alertComponent.GetAlertsAsync(new AlertRequestModel
			{
				NoPaging = true,
				ShowActiveAndInactive = true
			}); ;

			return View(alerts);

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
		[HttpPost]
		public async Task<ActionResult> Edit(AlertViewModel viewmodel)
		{
			await _alertComponent.UpdateAlertAsync(viewmodel.ToModel());
			return RedirectToAction("Index");
		}
	}
}