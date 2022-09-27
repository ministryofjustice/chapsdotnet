using System.Collections.Generic;
using System.Linq;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Data.Entities;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Areas.Admin.Controllers
{   
    [Area("Admin")]
    public class MPsController : Controller
    {
        private readonly IMPComponent _mpComponent;
        private readonly ISalutationComponent _salutationComponent;

        public MPsController(IMPComponent mpComponent, ISalutationComponent salutationComponent)
        {
            _mpComponent = mpComponent;
            _salutationComponent = salutationComponent;
        }

        public async Task<IActionResult> Index(
            string nameSearchTerm,
            string addressSearchTerm,
            string emailSearchTerm,
            int page = 1
        )
        {
            var pagedResult = await _mpComponent.GetMPsAsync(new MPRequestModel
                {
                    PageNumber = page,
                    PageSize = 10,
                    ShowActiveAndInactive = true,
                    nameSearchTerm = nameSearchTerm,
                    addressSearchTerm = addressSearchTerm,
                    emailSearchTerm = emailSearchTerm
                }
            );

            foreach (var item in pagedResult.Results!)
            {
                if (item != null)
                {
                    item.DisplayFullName = DisplayFullName(item.MPId).Result;
                }
            }
            return View(pagedResult.Results);
        }

        public async Task<ActionResult> Create()
        {
            var model = new MPViewModel();
            var salutations = await _salutationComponent.GetSalutationsAsync(new SalutationRequestModel
            {
                PageSize = 75,
                ShowActiveAndInactive = false
            });

            if (salutations.Results != null)
            {
                model.SalutationList = new SelectList(salutations.Results, "SalutationId", "Detail");
            }

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
            var salutations = await _salutationComponent.GetSalutationsAsync(new SalutationRequestModel
            {
                PageSize = 75,
                ShowActiveAndInactive = false
            });

            var viewModel = model.ToViewModel();
            if (salutations.Results != null)
            {
                viewModel.SalutationList = new SelectList(salutations.Results, "SalutationId", "Detail");
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MPViewModel viewModel)
        {
            await _mpComponent.UpdateMPAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }

        public async Task<string> DisplayFullName(int id)
        {
            var mpmodel = await _mpComponent.GetMPAsync(id);
            var mp = mpmodel.ToViewModel();
            string? salutation = null;
            if (mp.SalutationId != null)
            {
                salutation = _salutationComponent.GetSalutationAsync((int)mp.SalutationId).Result.Detail;
            }
            else
            {
                salutation = String.Empty;
            }
           
            List<string> nameParts = new List<string>();

            if (mp.RtHon == true)
            {
                nameParts.Add("RtHon");
            }

            nameParts.Add(salutation!);
            nameParts.Add(mp.FirstNames != null ? mp.FirstNames : null!);
            nameParts.Add(mp.Surname);
            nameParts.Add(mp.Suffix != null ? mp.Suffix : null!);

            return string.Join(" ", nameParts.Where(s => !string.IsNullOrEmpty(s)));

        }
    }
}
