using ChapsDotNET.Business.Interfaces;
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

        public async Task<IActionResult> Index(string sortOrder)
        {
            var model = new UserAdminViewModel();
            model.SortOrder = sortOrder;

            var users = await _userComponent.GetUsersAsync(sortOrder);
            model.Users = users;
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

            if (result.warning == "Success")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit", "Users", new { userId = result.userId, warning=result.warning });
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
                warning = warning
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
        public async Task<IActionResult> Edit(UserViewModel model)
        {
   
            await _userComponent.UpdateUserAsync(model.ToModel());

            return RedirectToAction("Index");
        }
    }
}