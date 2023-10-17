using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MPsController : Controller
    {
        private readonly IMPComponent _mpComponent;
        private readonly ISalutationComponent _salutationComponent;
        private readonly IUserComponent _userComponent;

        public MPsController(IMPComponent mpComponent, ISalutationComponent salutationComponent, IUserComponent userComponent)
        {
            _mpComponent = mpComponent;
            _salutationComponent = salutationComponent;
            _userComponent = userComponent;
        }

        public async Task<IActionResult> Index(string nameFilterTerm, string addressFilterTerm, string emailFilterTerm, bool activeFilter, string sortOrder, int page = 1)
        {
            var pagedResult = await _mpComponent.GetMPsAsync(new MPRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = false,
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
        [ValidateAntiForgeryToken]
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
            model.DisplayFullName = await DisplayFullName(model.MPId);
            var emailAddress = "CHAPS-Support@digital.justice.gov.uk";
            var salutations = await _salutationComponent.GetSalutationsAsync(new SalutationRequestModel
            {
                PageSize = 75,
                ShowActiveAndInactive = false
            });

            if (model.Active == false)
            {
                var errorMessage = ($"You cannot edit '{model.DisplayFullName}' as the record has been deactivated, please  email <a href='mailto:{emailAddress}'>{emailAddress}</a> to re-activate it.");
                return RedirectToAction("InactiveMpError", new { errorMessage });
            }

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
            var model = await _mpComponent.GetMPAsync(viewModel.MPId);

            if (viewModel.Active != model.Active && model.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }

            await _mpComponent.UpdateMPAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }

        public async Task<string> DisplayFullName(int id)
        {
            var mpmodel = await _mpComponent.GetMPAsync(id);

            if(mpmodel == null)
            {
                throw new NullReferenceException($"Error retrieving MPModel Id: {id}.");
            }

            var mp = mpmodel.ToViewModel();
            string? salutation = null;
            if (mp.SalutationId != null)
            {
                var salutationResult = await _salutationComponent.GetSalutationAsync((int)mp.SalutationId);
                salutation = salutationResult?.Detail ?? String.Empty;
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

        public async Task<ActionResult> Deactivate(MPViewModel viewmodel, bool x = false)
        {
            viewmodel.DisplayFullName = await DisplayFullName(viewmodel.MPId);
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(MPViewModel viewModel)
        {
            viewModel.Active = false;
            viewModel.DeactivatedOn = DateTime.Now;
            var user = await _userComponent.GetUserByNameAsync(User.Identity!.Name);         
            viewModel.DeactivatedByID = user.UserId;

            await _mpComponent.UpdateMPAsync(viewModel.ToModel());

            return RedirectToAction("index");
        }

        public ActionResult InactiveMpError(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetActiveTrue(int id)
        {
            var mp = await _mpComponent.GetMPAsync(id);
            if (mp != null)
            {
                var model = new MPModel
                {
                    MPId = mp.MPId,
                    RtHon = mp.RtHon,
                    SalutationId = mp.SalutationId,
                    FirstNames = mp.FirstNames,
                    Surname = mp.Surname,
                    Email = mp.Email,
                    Suffix = mp.Suffix,
                    AddressLine1 = mp.AddressLine1,
                    AddressLine2 = mp.AddressLine2,
                    AddressLine3 = mp.AddressLine3,
                    Town = mp.Town,
                    County = mp.County,
                    Postcode = mp.Postcode,
                    Active = true,
                    DeactivatedByID = mp.DeactivatedByID,
                    DeactivatedOn = mp.DeactivatedOn,
                };

                await _mpComponent.UpdateMPAsync(model);
            }
            return Json(new { success = true });
        }
    }
}