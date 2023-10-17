using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Data.Entities;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeadSubjectsController : Controller
    {
        private readonly ILeadSubjectComponent _leadSubjectComponent;
        private readonly IUserComponent _userComponent;

        public LeadSubjectsController(ILeadSubjectComponent leadSubjectComponent, IUserComponent userComponent)
        {
            _leadSubjectComponent = leadSubjectComponent;
            _userComponent = userComponent;
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
            viewmodel.Active = false;
            viewmodel.deactivated = DateTime.Now;
            var user = await _userComponent.GetUserByNameAsync(User.Identity!.Name);
            viewmodel.deactivatedBy = user.DisplayName;

            await _leadSubjectComponent.UpdateLeadSubjectAsync(viewmodel.ToModel());

            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetActiveTrue(int id)
        {
            var leadSubject = await _leadSubjectComponent.GetLeadSubjectAsync(id);
            if (leadSubject != null)
            {
                var model = new LeadSubjectModel
                {
                    LeadSubjectId = leadSubject.LeadSubjectId,
                    Detail = leadSubject.Detail,
                    Active = true
                };

                await _leadSubjectComponent.UpdateLeadSubjectAsync(model);
            }
            return Json(new { success = true });
        }
    }
}