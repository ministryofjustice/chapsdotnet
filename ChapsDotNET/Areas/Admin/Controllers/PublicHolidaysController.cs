﻿using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PublicHolidaysController : Controller
    {
        private readonly IPublicHolidayComponent _publicHolidayComponent;

        public PublicHolidaysController(IPublicHolidayComponent publicHolidayComponent)
        {
            _publicHolidayComponent = publicHolidayComponent;
        }

        public async Task<IActionResult> Index()
        {
            var pagedResult = await _publicHolidayComponent
                .GetPublicHolidaysAsync(new PublicHolidayRequestModel
                {
                    NoPaging = true
                });

            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new PublicHolidayViewModel
            {
                Date = DateTime.Now.AddDays(1)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PublicHolidayViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _publicHolidayComponent.AddPublicHolidayAsync(viewModel.ToModel());
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _publicHolidayComponent.GetPublicHolidayAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PublicHolidayViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _publicHolidayComponent.UpdatePublicHolidayAsync(viewModel.ToModel());
                return RedirectToAction("index");
            }
            return View();
        }
    }
}
