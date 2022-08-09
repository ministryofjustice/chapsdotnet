using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
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
                    PageNumber = 5,
                    PageSize = 10,
                    ShowActiveAndInactive = true
                });

            return View(pagedResult.Results);
        }
    }
}
