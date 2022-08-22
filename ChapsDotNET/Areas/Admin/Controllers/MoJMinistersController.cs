using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoJMinistersController : Controller
    {
        private readonly IMoJMinisterComponent _mojMinisterComponent;

        public MoJMinistersController(IMoJMinisterComponent mojMinisterComponent)
        {
            _mojMinisterComponent = mojMinisterComponent;
        }

        public async Task<IActionResult> Index()
        {
            var pagedResult = await _mojMinisterComponent.GetMoJMinistersAsync(new MoJMinisterRequestModel
                {
                    PageNumber = 1,
                    PageSize = 100,
                    ShowActiveAndInactive = true
                }
            );
            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new MoJMinisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MoJMinisterViewModel viewModel)
        {
            await _mojMinisterComponent.AddMoJMinisterAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _mojMinisterComponent.GetMoJMinisterAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MoJMinisterViewModel viewModel)
        {
            await _mojMinisterComponent.UpdateMoJMinisterAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }
    }
}
