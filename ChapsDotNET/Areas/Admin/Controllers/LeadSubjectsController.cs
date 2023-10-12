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
    public class LeadSubjectsController : Controller
    {
        private readonly ILeadSubjectComponent _leadSubjectComponent;

        public LeadSubjectsController(ILeadSubjectComponent leadSubjectComponent)
        {
            _leadSubjectComponent = leadSubjectComponent;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedResult = await _leadSubjectComponent.GetLeadSubjectsAsync(new LeadSubjectRequestModel
            {
                NoPaging = true,
                ShowActiveAndInactive = true
            }
            );
            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new LeadSubjectViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(LeadSubjectViewModel viewModel)
        {
            await _leadSubjectComponent.AddLeadSubjectAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _leadSubjectComponent.GetLeadSubjectAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(LeadSubjectViewModel viewModel)
        {
            var model = await _leadSubjectComponent.GetLeadSubjectAsync(viewModel.LeadSubjectId);

            if (viewModel.Active != model.Active && model.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }

            if (viewModel.Active != model.Active && model.Active == false)
            {
                viewModel.deactivated = null;
                viewModel.deactivatedBy = null;
                await _leadSubjectComponent.UpdateLeadSubjectAsync(viewModel.ToModel());
                return RedirectToAction("index");
            }

            viewModel.deactivated = model.deactivated;
            viewModel.deactivatedBy = model.deactivatedBy;

            await _leadSubjectComponent.UpdateLeadSubjectAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }

        public ActionResult Deactivate(LeadSubjectViewModel viewmodel, bool x = false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(LeadSubjectViewModel viewmodel)
        {
            var jsonModel = TempData["viewModel"] as string;

            if (jsonModel != null)
            {
                var model = JsonConvert.DeserializeObject<LeadSubjectViewModel>(jsonModel);
                var user = User.Identity!.Name;

                var subjectModel = new LeadSubjectModel
                {
                    LeadSubjectId = viewmodel.LeadSubjectId,
                    Detail = viewmodel.Detail,
                    Active = viewmodel.Active,
                    deactivated = DateTime.Now,
                    deactivatedBy = user
                };

                await _leadSubjectComponent.UpdateLeadSubjectAsync(subjectModel);
            }

            return RedirectToAction("index");
        }
    }
}