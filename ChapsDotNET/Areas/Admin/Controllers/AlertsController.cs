using ChapsDotNET.Business.Components;
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
        private readonly IAlertComponent _AlertComponent;

        public AlertsController(IAlertComponent AlertComponent)
        {
            _AlertComponent = AlertComponent;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedResult = await _AlertComponent.GetAlertsAsync(new AlertRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = true
            }
            );
            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new AlertViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AlertViewModel viewModel)
        {
            await _AlertComponent.AddAlertAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _AlertComponent.GetAlertAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AlertViewModel viewModel)
        {
            await _AlertComponent.UpdateAlertAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }
    }
}


