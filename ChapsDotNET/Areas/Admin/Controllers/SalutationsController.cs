using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalutationsController : Controller
    {
        private readonly ISalutationComponent _salutationComponent;

        public SalutationsController(ISalutationComponent salutationComponent)
        {
            _salutationComponent = salutationComponent;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedResult = await _salutationComponent
                .GetSalutationsAsync(new SalutationRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = false
            });

            return View(pagedResult);
        }

        public ActionResult Create()
        {
            var model = new SalutationViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(SalutationViewModel viewModel)
        {
            await _salutationComponent.AddSalutationAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _salutationComponent.GetSalutationAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(SalutationViewModel viewModel)
        {
            var model = await _salutationComponent.GetSalutationAsync(viewModel.SalutationId);

            if (viewModel.Active != model.Active && model.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }

            await _salutationComponent.UpdateSalutationAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }

        public ActionResult Deactivate(SalutationViewModel viewmodel, bool x = false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(SalutationViewModel viewModel)
        {
            viewModel.Active = false;
            await _salutationComponent.UpdateSalutationAsync(viewModel.ToModel());

            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetActiveTrue(int id)
        {
            var salutation = await _salutationComponent.GetSalutationAsync(id);
            if (salutation != null)
            {
                var model = new SalutationModel
                {
                    SalutationId = salutation.SalutationId,
                    Detail = salutation.Detail,
                    Active = true
                };

                await _salutationComponent.UpdateSalutationAsync(model);
             }
            return Json(new { success = true });
        }
    }
}
