﻿using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
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
        public async Task<ActionResult> Create(SalutationModel model)
        {
             await _salutationComponent.AddSalutationAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Update(int id)
        {
            var model = await _salutationComponent.GetSalutationAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(SalutationModel model)
        {
            await _salutationComponent.UpdateSalutationAsync(model);
             return RedirectToAction("index");
        }

    }
}
