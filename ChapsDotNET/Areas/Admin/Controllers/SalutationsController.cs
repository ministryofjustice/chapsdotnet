
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ActionAlertModel = ChapsDotNET.Frontend.Components.Alert.AlertModel;
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
            var model = new SalutationsAdminViewModel {};
            var pagedResults = await _salutationComponent
                .GetSalutationsAsync(new SalutationRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = false
            });

            ActionAlertModel? alert = null;
            if (TempData["alertStatus"] != null && TempData["alertContent"] != null && TempData["alertSummary"] != null)
            {
                alert = new ActionAlertModel
                {
                    Type = ActionAlertModel.GetAlertTypeFromStatus(TempData["alertStatus"] as string),
                    Content = TempData["alertContent"] as string,
                    Summary = TempData["alertSummary"] as string
                };
            }
            model.Salutations = pagedResults;
            model.Alert = alert;
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new SalutationViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(SalutationViewModel viewModel)
        {
            int id = await _salutationComponent.AddSalutationAsync(viewModel.ToModel());
            SalutationModel salutation = await _salutationComponent.GetSalutationAsync(id);
            if (salutation != null && salutation.Detail != null)
            {
                TempData["alertContent"] = $"Salutation {salutation.Detail} created successfully";
                TempData["alertSummary"] = $"Salutation created successfully";
                TempData["alertStatus"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["alertContent"] = $"Unable to create salutation";
                TempData["alertSummary"] = $"Unable to create salutation";
                TempData["alertStatus"] = "error";
                return RedirectToAction("Index");
            }
 
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _salutationComponent.GetSalutationAsync(id);
            ActionAlertModel? alert = null;
            if (TempData["alertStatus"] != null && TempData["alertContent"] != null && TempData["alertSummary"] != null)
            {
                alert = new ActionAlertModel
                {
                    Type = ActionAlertModel.GetAlertTypeFromStatus(TempData["alertStatus"] as string),
                    Content = TempData["alertContent"] as string,
                    Summary = TempData["alertSummary"] as string
                };
            }
            var viewModel = model.ToViewModel();
            viewModel.Alert = alert;
            return View(viewModel);
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

            SalutationModel salutation = await _salutationComponent.UpdateSalutationAsync(viewModel.ToModel());
            TempData["alertContent"] = $"Salutation {salutation.Detail} updated successfully";
            TempData["alertSummary"] = $"Salutation updated successfully";
            TempData["alertStatus"] = "success";
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(SalutationViewModel viewmodel, bool x = false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(SalutationViewModel viewModel)
        {
            viewModel.Active = false;
            SalutationModel salutation = await _salutationComponent.UpdateSalutationAsync(viewModel.ToModel());
            TempData["alertContent"] = $"Salutation {salutation.Detail} deactivated";
            TempData["alertSummary"] = $"Salutation deactivated";
            TempData["alertStatus"] = "success";
            return RedirectToAction("Index");
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
