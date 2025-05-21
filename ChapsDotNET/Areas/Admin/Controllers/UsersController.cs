using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Frontend.Components.Alert;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
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

        public async Task<IActionResult> Index(string sortOrder, string? page)
        {
            int pageSize = 25;
            int currentPage = 1;
            if (page != null) { _ = int.TryParse(page, out currentPage); }
            var model = new UserAdminViewModel { SortOrder = sortOrder };
            AlertModel? alert = null;
            if (TempData["alertStatus"] != null && TempData["alertContent"] != null && TempData["alertSummary"] != null)
            {
               alert = new AlertModel { 
                   Type = AlertModel.GetAlertTypeFromStatus(TempData["alertStatus"] as string),
                   Content = TempData["alertContent"] as string, 
                   Summary = TempData["alertSummary"] as string
               };
            }
            var users = await _userComponent.GetUsersAsync(new UserRequestModel
            {
                PageNumber = currentPage,
                PageSize = pageSize,
                SortOrder = sortOrder,
            });
            model.Users = users;
            model.Alert = alert;
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
                Warning = warning
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
            model.TeamList = new SelectList(teams.Results, "TeamId", "Name");
            model.RoleList = new SelectList(roles.Results, "RoleStrength", "Detail");
           
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
