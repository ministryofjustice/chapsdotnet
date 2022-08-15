﻿using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
        {
            var pagedResult = await _campaignComponent.GetCampaignsAsync(new CampaignRequestModel
                {
                    PageNumber = 1,
                    PageSize = 10,
                    ShowActiveAndInactive = true
                }
            );
            return View(pagedResult.Results);
        }

        //public ActionResult Create()
        //{
        //    var model = new SalutationViewModel();
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(SalutationViewModel viewModel)
        //{
        //    await _salutationComponent.AddSalutationAsync(viewModel.ToModel());
        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> Edit(int id)
        //{
        //    var model = await _salutationComponent.GetSalutationAsync(id);
        //    return View(model.ToViewModel());
        //}

        //[HttpPost]
        //public async Task<ActionResult> Edit(SalutationViewModel viewModel)
        //{
        //    await _salutationComponent.UpdateSalutationAsync(viewModel.ToModel());
        //    return RedirectToAction("index");
        //}
    }
}
