using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class MPsController : Controller
    {
        private readonly IMPComponent _mpComponent;

        public MPsController(IMPComponent mpComponent)
        {
            _mpComponent = mpComponent;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedResult = await _mpComponent.GetMPsAsync(new MPRequestModel
                {
                    PageNumber = page,
                    PageSize = 10,
                    ShowActiveAndInactive = true
                }
            );
            return View(pagedResult.Results);
        }

        public async Task<ActionResult> Create()
        {
            var model = new MPViewModel();
            model.SalutationList = new SelectList(await _mpComponent.GetActiveSalutationsListAsync().ConfigureAwait(false), "SalutationID", "Detail");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MPViewModel viewModel)
        {
            await _mpComponent.AddMPAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _mpComponent.GetMPAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MPViewModel viewModel)
        {
            await _mpComponent.UpdateMPAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }
    }
}
