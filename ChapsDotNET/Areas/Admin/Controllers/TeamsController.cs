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
            var jsonModel = TempData["viewModel"] as string;

            if (jsonModel!= null)
            {
                var model = JsonConvert.DeserializeObject<TeamViewModel>(jsonModel);
                var user = User.Identity!.Name;

                var teamModel = new TeamModel
                {
                    TeamId = viewmodel.TeamId,
                    Acronym = viewmodel.Acronym,
                    Active = viewmodel.Active,
                    Email = viewmodel.Email,
                    IsOgd = viewmodel.IsOgd,
                    IsPod = viewmodel.IsPod,
                    Name = viewmodel.Name,
                    deactivated = DateTime.Now,
                    deactivatedBy = user
                };


                await _teamComponent.UpdateTeamAsync(teamModel);

            }
           

            return RedirectToAction("index");

        }


    }
}


