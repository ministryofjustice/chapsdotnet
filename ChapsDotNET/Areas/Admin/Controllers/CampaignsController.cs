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
    public class CampaignsController : Controller
    {
        private readonly ICampaignComponent _campaignComponent;

        public CampaignsController(ICampaignComponent CampaignComponent)
        {
            _campaignComponent = CampaignComponent;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedResult = await _campaignComponent.GetCampaignsAsync(new CampaignRequestModel
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
            var model = new CampaignViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CampaignViewModel viewModel)
        {
            await _campaignComponent.AddCampaignAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _campaignComponent.GetCampaignAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CampaignViewModel viewModel)
        {
            var model = await _campaignComponent.GetCampaignAsync(viewModel.CampaignId);

            if (viewModel.Active != model.Active && model.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }

            await _campaignComponent.UpdateCampaignAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public ActionResult Deactivate(CampaignViewModel viewmodel, bool x = false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(CampaignViewModel viewmodel)
        {
            viewmodel.Active = false;
            await _campaignComponent.UpdateCampaignAsync(viewmodel.ToModel());

            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetActiveTrue(int id)
        {
            var campaign = await _campaignComponent.GetCampaignAsync(id);
            if (campaign != null)
            {
                var model = new CampaignModel
                {
                    CampaignId = campaign.CampaignId,
                    Detail = campaign.Detail,
                    Active = campaign.Active
                };

                await _campaignComponent.UpdateCampaignAsync(model);
            }
            return Json(new { success = true });
        }
    }
}
