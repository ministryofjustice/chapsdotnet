using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Entities;
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
            var model = new SalutationModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(SalutationModel model)
        {
            _salutationComponent.AddSalutationAsync(model);
            return RedirectToAction("Index");
        }

        public ActionResult Update(int Id)
        {
      
            return View("Edit");

        }
    }
}
