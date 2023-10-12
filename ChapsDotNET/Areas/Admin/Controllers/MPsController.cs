﻿using System.Linq;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Data.Entities;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;

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

        public async Task<IActionResult> Index(string nameFilterTerm, string addressFilterTerm, string emailFilterTerm, bool activeFilter, string sortOrder, int page = 1)
        {
            var pagedResult = await _mpComponent.GetMPsAsync(new MPRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = true,
                nameFilterTerm = nameFilterTerm,
                addressFilterTerm = addressFilterTerm,
                emailFilterTerm = emailFilterTerm,
                sortOrder = sortOrder
            });

            foreach (var item in pagedResult.Results!)
            {
                if (item != null)
                {
                    item.DisplayFullName = DisplayFullName(item.MPId).Result;

                }
            }

            var pagination = new PaginationInfo
            {
                CurrentPage = pagedResult.CurrentPage,
                TotalItems = pagedResult.RowCount,
                TotalPages = pagedResult.PageCount,
                ItemsPerPage = pagedResult.PageSize
            };

            var indexVM = new IndexViewModel
            {
                MPs = pagedResult,
                Pagination = pagination
            };

            return View(indexVM);
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetFilteredMPs([FromBody] MPRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var pagedResult = await _mpComponent.GetFilteredMPsAsync(model);
                var response = new MPsAdminViewModel
                {
                    MPs = pagedResult.Results,
                    Pagination = new PaginationInfo
                    {
                        CurrentPage = pagedResult.CurrentPage,
                        TotalPages = pagedResult.PageCount,
                        TotalItems = pagedResult.RowCount,
                        ItemsPerPage = pagedResult.PageSize
                    }
                };

                foreach (var item in pagedResult.Results!)
                {
                    if (item != null)
                    {
                        item.DisplayFullName = DisplayFullName(item.MPId).Result;
                    }
                }
                return Json(response);
            }
            else
            {
                return View("index");
            }
        }


        public async Task<ActionResult> Create()
        {
            var model = new MPViewModel();
            var salutations = await _salutationComponent.GetSalutationsAsync(new SalutationRequestModel
            {
                PageSize = 1000,
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

            if (viewModel.Active != viewModel.Active && viewModel.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }

            if (viewModel.Active != viewModel.Active && viewModel.Active == false)
            {
                viewModel.DeactivatedOn = null;
                viewModel.DeactivatedByID = null;
                await _mpComponent.UpdateMPAsync(viewModel.ToModel());
                return RedirectToAction("index");
            }

            viewModel.DeactivatedOn = viewModel.DeactivatedOn;
            viewModel.DeactivatedByID = viewModel.DeactivatedByID;

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

        public ActionResult Deactivate(MPViewModel viewmodel, bool x = false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(MPViewModel viewModel)
        {
            viewModel.Active = false;
            viewModel.DeactivatedOn = DateTime.Now;

            //string currentUserId = _userManager.GetUserId(User);
            //if (!string.IsNullOrEmpty(currentUserId))
            //{
            //    viewModel.DeactivatedByID = Int32.Parse(currentUserId);
            //}
            //else
            //{
            //    // handle null userID
            //}


            await _mpComponent.UpdateMPAsync(viewModel.ToModel());

            return RedirectToAction("index");
        }
    }
}