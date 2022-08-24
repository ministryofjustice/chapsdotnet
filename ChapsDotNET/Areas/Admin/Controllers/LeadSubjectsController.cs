using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

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
                PageNumber = page,
                PageSize = 10,
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
            await _leadSubjectComponent.UpdateLeadSubjectAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }
    }
}


