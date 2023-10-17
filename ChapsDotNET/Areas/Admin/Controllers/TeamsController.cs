using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;


namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly ITeamComponent _teamComponent;
        private readonly IUserComponent _userComponent;

        public TeamsController(ITeamComponent teamComponent, IUserComponent userComponent)
        {
            _teamComponent = teamComponent;
            _userComponent = userComponent;
        }
        public async Task<IActionResult> Index()
        {
            var pagedResult = await _teamComponent
                .GetTeamsAsync(new TeamRequestModel
                {
                    NoPaging = true,
                    ShowActiveAndInactive = true
                });

            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new TeamViewModel();

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Create(TeamViewModel viewModel)
        {
            await _teamComponent.AddTeamAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Edit(int id)
        {
            var model = await _teamComponent.GetTeamAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TeamViewModel viewModel)
        {
            var model = await _teamComponent.GetTeamAsync(viewModel.TeamId);

            if (viewModel.Active != model.Active && model.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }

            if (viewModel.Active != model.Active && model.Active == false)
            {
                viewModel.deactivated = null;
                viewModel.deactivatedBy = null;
                await _teamComponent.UpdateTeamAsync(viewModel.ToModel());
                return RedirectToAction("index");
            }

            viewModel.deactivated = model.deactivated;
            viewModel.deactivatedBy = model.deactivatedBy;

            await _teamComponent.UpdateTeamAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }


        public ActionResult Deactivate(TeamViewModel viewmodel, bool x= false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(TeamViewModel viewmodel)
        {
            viewmodel.Active = false;
            viewmodel.deactivated = DateTime.Now;
            var user = await _userComponent.GetUserByNameAsync(User.Identity!.Name);
            viewmodel.deactivatedBy = user.DisplayName;

            await _teamComponent.UpdateTeamAsync(viewmodel.ToModel());
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetActiveTrue(int id)
        {
            var team = await _teamComponent.GetTeamAsync(id);
            if (team != null)
            {
                var model = new TeamModel
                {
                    TeamId = team.TeamId,
                    Name = team.Name,
                    Active = true,
                    Acronym = team.Acronym,
                    Email = team.Email,
                    IsOgd = team.IsOgd,
                    IsPod = team.IsPod,
                    deactivated = team.deactivated,
                    deactivatedBy = team.deactivatedBy
                };

                await _teamComponent.UpdateTeamAsync(model);
            }
            return Json(new { success = true });
        }
    }
}