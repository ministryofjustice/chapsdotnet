using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
        {
            var pagedResult = await _salutationComponent
                .GetSalutationsAsync(new SalutationRequestModel
            {
                PageNumber = 5,
                PageSize = 10,
                ShowActiveAndInactive = true
            });

            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new SalutationViewModel();

            return View(model);
        }

       
        [HttpPost]
        public async Task<ActionResult> Create(SalutationViewModel model)
        {
             await _salutationComponent.AddSalutationAsync(new SalutationModel
             {
                 Detail = model.Detail,
                 Active = model.Active,
                 SalutationId = model.SalutationId
             });

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Update(int id)
        {
            var model = await _salutationComponent.GetSalutationAsync(id);

            var viewModel = new SalutationViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                SalutationId = model.SalutationId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(SalutationViewModel model)
        {
            await _salutationComponent.UpdateSalutationAsync(new SalutationModel
            {
                Detail = model.Detail,
                Active = model.Active,
                SalutationId = model.SalutationId
            });

            return RedirectToAction("index");
        }

    }
}
