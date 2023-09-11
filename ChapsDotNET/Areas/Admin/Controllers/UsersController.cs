using System;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using Microsoft.AspNetCore.Mvc;
using ChapsDotNET.Models;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChapsDotNET.Common.Mappers;

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
            var model = new UsersViewModel();
            model.SortOrder = sortOrder;

            var users = await _userComponent.GetUsersAsync(sortOrder);
            model.Users = users;
            return View(model);
        }

        // todo - Create

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
            await _userComponent.AddUserAsync(model.ToModel());
            return RedirectToAction("index");
        }

        // todo - Edit
    }
}