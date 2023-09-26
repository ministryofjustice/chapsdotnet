using System.Globalization;
using System.Text.RegularExpressions;
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
			var model = new AlertViewModel
			{
				WarnStartString = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
				EventStartString = DateTime.Now.ToString("dd/MM/yyyy HH:mm")

			};
			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(AlertViewModel viewModel)
		{
			if(viewModel.Message != null)
			{
                var message = viewModel.Message!;
                string noHtml = Regex.Replace(message, "<.*?>", String.Empty).Trim();

                if (String.IsNullOrWhiteSpace(noHtml))
                {
                    ModelState.AddModelError("Message", "Message is required.");
                }
            }


            if (ModelState.IsValid)
			{
				try
				{
					DateTime.TryParseExact(viewModel.WarnStartString!,
					"dd/MM/yyyy HH:mm",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime warnStart);
					viewModel.WarnStart = warnStart;


					DateTime.TryParseExact(viewModel.EventStartString!,
					"dd/MM/yyyy HH:mm",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime eventStart);
					viewModel.EventStart = eventStart;
				}
				catch (FormatException)
				{
					throw new FormatException("Could not parse Alert time, incorrect format");
				}
                await _alertComponent.AddAlertAsync(viewModel.ToModel());
                return RedirectToAction("Index");
            }
			else
			{
				return View(viewModel);
			}
            
        }
		
		public async Task<IActionResult> Edit(int id)
		{
			var model = await _alertComponent.GetAlertAsync(id);
			var viewModel = model.ToViewModel();
			viewModel.WarnStartString = viewModel.WarnStart.ToString("dd/MM/yyyy HH:mm");
            viewModel.EventStartString = viewModel.EventStart.ToString("dd/MM/yyyy HH:mm");

            return View(viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> Edit(AlertViewModel viewmodel)
        {
            var message = viewmodel.Message!;
            string noHtml = Regex.Replace(message, "<.*?>", String.Empty).Trim();

            if (String.IsNullOrWhiteSpace(noHtml))
            {
                ModelState.AddModelError("Message", "Message is required.");
            }

            if (ModelState.IsValid)
			{
				try
				{

					DateTime.TryParseExact(viewmodel.WarnStartString!,
					"dd/MM/yyyy HH:mm",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime warnStart);
					viewmodel.WarnStart = warnStart;

					DateTime.TryParseExact(viewmodel.EventStartString!,
					  "dd/MM/yyyy HH:mm",
					  CultureInfo.InvariantCulture,
					  DateTimeStyles.None,
					  out DateTime eventStart);
					viewmodel.EventStart = eventStart;
				}
				catch (FormatException)
				{
					throw new FormatException("Could not parse Alert time, incorrect format");
				}
				await _alertComponent.UpdateAlertAsync(viewmodel.ToModel());
				return RedirectToAction("Index");
			}
			else
			{ 
				return View(viewmodel);
			}
		}
	}
}