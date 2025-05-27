using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Frontend.Components.Breadcrumbs;
using ChapsDotNET.Frontend.Components.Button;
using ChapsDotNET.Frontend.Components.Checkbox;
using ChapsDotNET.Frontend.Components.Heading;
using ChapsDotNET.Frontend.Components.ListFilter;
using ChapsDotNET.Frontend.Components.Table;
using ChapsDotNET.Frontend.Components.TextInput;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MPsController : IndexController<MPModel>
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

        public async Task<IActionResult> Index(string nameFilterTerm, string addressFilterTerm, string emailFilterTerm, string activeFilter, string sortOrder, int page = 1)
        {
            var resultsData = await _mpComponent.GetFilteredMPsAsync(new MPRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = activeFilter != null ? true : false,
                nameFilterTerm = nameFilterTerm,
                addressFilterTerm = addressFilterTerm,
                emailFilterTerm = emailFilterTerm,
                sortOrder = sortOrder
            });

            var results = resultsData.Results!;
            var resultsCount = results.Count;

            // Breadcrumbs
            List<BreadcrumbModel> breadcrumbs = [
                new BreadcrumbModel { Label = "Home", Url = Url.Action("Index", "Home", new {area =""})!},
                new BreadcrumbModel { Label = "Admin", Url = Url.Action("Index", "Admin", new {area =""})!},
            ];
            var breadcrumbsModel = new BreadcrumbsModel(breadcrumbs);
            
            // Heading
            var headingModel = new HeadingModel { Title = "Members of Parliament (MPs)", Button = new ButtonModel { Element = ButtonModel.ValidElementTypes.anchor, Type = ButtonModel.ValidButtonStyles.secondary, Label = "Add a new member of parliament", Url = Url.Action("Create", "MPs") } };
            
            // Table

            //// Table Headers
            List<HeaderCell> headers = [
                new HeaderCell { Content = "Name", Sort = GetSort(sortOrder, "name"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "name") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Address", Sort = GetSort(sortOrder, "address"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "address") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Email", Sort = GetSort(sortOrder, "email"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "email") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Active",Sort = GetSort(sortOrder, "deactiv"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "deactiv") }), Scope = RowScopes.col},
            ];

            //// Table body
            var rows = new List<Row>();

            foreach(var mp in results!)
            {
                var row = new Row
                {
                    RowContent = [
                        new BodyCell {Content = DisplayFullName(mp.MPId).Result, Url = $"/Admin/MPs/Edit/{mp.MPId}"},
                        new BodyCell {Content = mp.DisplaySingleLineAddress != null ? mp.DisplaySingleLineAddress : "Not set"},
                        new BodyCell {Content = mp.Email != null ? mp.Email : "Not set" },
                        new BodyCell {Content = mp.Active ? "Active" : "Deactivated", TagColour = mp.Active ? TagColours.blue : TagColours.grey }
                    ]
                };
                rows.Add(row);
            }

            var tableModel = GetTable("Existing MPs", headers, rows, resultsData, resultsCount);

            // Filter options
            var filters = new List<IFormFieldModel>
            {
                new TextInputModel {Label = "Name", Id = "nameFilterTerm", Value = nameFilterTerm},
                new TextInputModel {Label = "Address", Id = "addressFilterTerm", Value = addressFilterTerm},
                new TextInputModel {Label = "Email", Id = "emailFilterTerm", Value = emailFilterTerm},
                new CheckboxModel {Label = "Show inactive MPs", Id = "activeFilter", Checked = activeFilter != null ? true : false}
            };

            var resetUrl = Url.Action("Index", "MPs")!;

            // Pagination
            var model = new IndexViewModel
            {
               Breadcrumbs = breadcrumbsModel,
               Alert = GetAlerts(),
               Heading = headingModel,
               ListFilter = GetListFilter(resultsData, tableModel, filters, resetUrl)
            };

            return View(model);
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
            try
            {
                var mpId = await _mpComponent.AddMPAsync(viewModel.ToModel());
                var fullName = await DisplayFullName(mpId);
                TempData["alertContent"] = $"MP {fullName} created successfully";
                TempData["alertSummary"] = $"MP created successfully";
                TempData["alertStatus"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["alertContent"] = "Unable to add MP ";
                TempData["alertSummary"] = $"{e.Message}";
                TempData["alertStatus"] = "error";
                return RedirectToAction("Create", "MPs");
            }
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
                var errorMessage = ($"You cannot edit '{model.DisplayFullName}' as the record has been deactivated, please  email <a class='govuk-link' href='mailto:{emailAddress}'>{emailAddress}</a> to re-activate it.");
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
            var fullName = await DisplayFullName(viewModel.MPId);
            TempData["alertContent"] = $"MP {fullName} updated successfully";
            TempData["alertSummary"] = $"MP updated successfully";
            TempData["alertStatus"] = "success";
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
            var fullName = await DisplayFullName(viewModel.MPId);
            TempData["alertContent"] = $"MP {fullName} deactivated";
            TempData["alertSummary"] = $"MP deactivated";
            TempData["alertStatus"] = "success";

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