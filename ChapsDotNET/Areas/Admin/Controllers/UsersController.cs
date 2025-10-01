
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Frontend.Components.Alert;
using ChapsDotNET.Frontend.Components.Breadcrumbs;
using ChapsDotNET.Frontend.Components.Button;
using ChapsDotNET.Frontend.Components.Checkbox;
using ChapsDotNET.Frontend.Components.Heading;
using ChapsDotNET.Frontend.Components.ListFilter;
using ChapsDotNET.Frontend.Components.Table;
using ChapsDotNET.Frontend.Components.TextInput;
using ChapsDotNET.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : IndexController<UserModel>
    {
        private readonly IUserComponent _userComponent;
        private readonly ITeamComponent _teamComponent;
        private readonly IRoleComponent _rolecomponent;


        public UsersController(IUserComponent usercomponent, ITeamComponent teamcomponent, IRoleComponent rolecomponent)
        {
            _userComponent = usercomponent;
            _teamComponent = teamcomponent;
            _rolecomponent = rolecomponent;
        }

        public async Task<IActionResult> Index(string DisplayNameFilterTerm, int AccessLevelFilterTerm, string activeFilter, string sortOrder, int page = 1)
        {
            var resultsData = await _userComponent.GetUsersAsync(new UserRequestModel
            {
                PageNumber = page,
                PageSize = 10,
                ShowActiveAndInactive = activeFilter != null ? true : false,
                DisplayNameFilterTerm = DisplayNameFilterTerm,
                AccessLevelFilterTerm = AccessLevelFilterTerm,
                SortOrder = sortOrder
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
            var headingModel = new HeadingModel { Title = "Users", Button = new ButtonModel { Element = ButtonModel.ValidElementTypes.anchor, Type = ButtonModel.ValidButtonStyles.secondary, Label = "Add a new User", Url = Url.Action("Create", "Users") } };

            // Table

            //// Table Headers
            List<HeaderCell> headers = [
                new HeaderCell { Content = "User Login", Sort = GetSort(sortOrder, "UserLogin"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "UserLogin") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Display Name", Sort = GetSort(sortOrder, "Display_name"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "Display_name") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Team", Sort = GetSort(sortOrder, "Team"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "Team") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Role", Sort = GetSort(sortOrder, "Role"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "Role") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Email", Sort = GetSort(sortOrder, "email"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "email") }), Scope = RowScopes.col},
                new HeaderCell { Content = "Active",Sort = GetSort(sortOrder, "Last_Active"), Url = Url.Action("Index", new { sortOrder = ToggleSortOrder(sortOrder, "Last_Active") }), Scope = RowScopes.col},
            ];

            //// Table body
            var rows = new List<Row>();

            foreach (var user in results!)
            {
                var row = new Row
                {
                    RowContent = [
                        new BodyCell {Content = user.Name  != null ? user.Name : "Not set", Url = $"/Admin/Users/Edit/{user.UserId}"},
                        new BodyCell {Content = user.DisplayName != null ? user.DisplayName : "Not set"},
                        new BodyCell {Content = user.Team != null ? user.Team : "Not set"},
                        new BodyCell {Content = user.Role != null ? user.Role : "Not set"},
                        new BodyCell {Content = user.Email != null ? user.Email : "Not set" },
                        new BodyCell {Content = user.LastActive != null ?  user.LastActive.Humanize() : "Never" }
                    ]
                };
                rows.Add(row);
            }

            var tableModel = GetTable("Users", headers, rows, resultsData, resultsCount);

            // Filter options
            var filters = new List<IFormFieldModel>
            {
                new TextInputModel {Label = "Display Name", Id = "displayNameFilterTerm", Value = DisplayNameFilterTerm}
                //TODO  TextInputModel {Label = "Address", Id = "addressFilterTerm", Value = addressFilterTerm}
                };

            var resetUrl = Url.Action("Index", "Users")!;

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


        public async Task<IActionResult> Create()
        {
            var model = new UserViewModel();

            var roles = await _rolecomponent.GetRolesAsync(new RoleRequestModel
            {
                NoPaging = true
            });

            var teams = await _teamComponent.GetTeamsAsync(new TeamRequestModel
            {
                ShowActiveAndInactive = true,
                NoPaging = true
            });

            model.TeamList = new SelectList(teams.Results, "TeamId", "Name");
            model.RoleList = new SelectList(roles.Results, "RoleStrength", "Detail");
            model.RoleStrength = -2; // prevents 'deactive' from being the default selection
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            var result = await _userComponent.AddUserAsync(model.ToModel());
            var user = await _userComponent.GetUserByIdAsync(result.userId);

            if (result.warning == "Success")
            {
                TempData["alertContent"] = $"User {user.Name} created successfully";
                TempData["alertSummary"] = $"User created successfully";
                TempData["alertStatus"] = "success";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["alertContent"] = result.warning;
                TempData["alertSummary"] = $"Unable to create user";
                TempData["alertStatus"] = "error";
                return RedirectToAction("Edit", "Users", new { result.userId });
            }
        }

        [Route("Admin/Users/Edit/{userId:int}")]
        public async Task<IActionResult> Edit(int userId, string? warning ="")
        {
            var user = await _userComponent.GetUserByIdAsync(userId);
            var model = new UserViewModel
            {
                Name = user.Name,
                DisplayName = user.DisplayName,
                Email = user.Email,
                RoleStrength = user.RoleStrength,
                TeamId= user.TeamId,
            };

            var roles = await _rolecomponent.GetRolesAsync(new RoleRequestModel
            {
                NoPaging = true
            });

            var teams = await _teamComponent.GetTeamsAsync(new TeamRequestModel
            {
                ShowActiveAndInactive = true,
                NoPaging = true
            });
            //AlertModel? alert = null;
            //if (TempData["alertStatus"] != null && TempData["alertContent"] != null && TempData["alertSummary"] != null)
            //{
            //    alert = new AlertModel
            //    {
            //        Type = AlertModel.GetAlertTypeFromStatus(TempData["alertStatus"] as string),
            //        Content = TempData["alertContent"] as string,
            //        Summary = TempData["alertSummary"] as string
            //    };
            //}
            model.TeamList = new SelectList(teams.Results, "TeamId", "Name");
            model.RoleList = new SelectList(roles.Results, "RoleStrength", "Detail");
            model.Alert = null; // TODO Current alert code gives errors.
            return View(model);
        }

        [HttpPost]
        [Route("Admin/Users/Edit/{userId:int}")]
        public async Task<IActionResult> Edit(UserViewModel model, int userId)
        {
   
            await _userComponent.UpdateUserAsync(model.ToModel());
            var user = await _userComponent.GetUserByIdAsync(userId);
            TempData["alertContent"] = $"User {user.Name} updated successfully";
            TempData["alertSummary"] = $"User updated successfully";
            TempData["alertStatus"] = "success";
            return RedirectToAction("Index");
        }
    }
}
