using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly ITeamComponent _teamComponent;

        public TeamsController(ITeamComponent teamComponent)
        {
            _teamComponent = teamComponent;
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
            await _teamComponent.UpdateTeamAsync(viewModel.ToModel());

            return RedirectToAction("index");
        }
    }
}
