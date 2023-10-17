using ChapsDotNET.Business.Components;
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
    public class MoJMinistersController : Controller
    {
        private readonly IMoJMinisterComponent _mojMinisterComponent;
        private readonly IUserComponent _userComponent;

        public MoJMinistersController(IMoJMinisterComponent mojMinisterComponent, IUserComponent userComponent)
        {
            _mojMinisterComponent = mojMinisterComponent;
            _userComponent = userComponent;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedResult = await _mojMinisterComponent.GetMoJMinistersAsync(new MoJMinisterRequestModel
                {
                    ShowActiveAndInactive = true,
                    NoPaging = true
                }
            );
            return View(pagedResult.Results);
        }

        public ActionResult Create()
        {
            var model = new MoJMinisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MoJMinisterViewModel viewModel)
        {
            await _mojMinisterComponent.AddMoJMinisterAsync(viewModel.ToModel());
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _mojMinisterComponent.GetMoJMinisterAsync(id);
            var emailAddress = "CHAPS-Support@digital.justice.gov.uk";

            if (model.Active == false)
            {
                var errorMessage = ($"You cannot edit '{model.Name}' as the record has been deactivated, please  email <a href='mailto:{emailAddress}'>{emailAddress}</a> to re-activate it.");
                return RedirectToAction("inactiveMoJMinisterError", new { errorMessage });
            }
            var viewModel = model.ToViewModel();
            return View(viewModel);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(MoJMinisterViewModel viewModel)
        {
            var model = await _mojMinisterComponent.GetMoJMinisterAsync(viewModel.MoJMinisterId);

            if (viewModel.Active != model.Active && model.Active == true)
            {
                TempData["viewModel"] = JsonConvert.SerializeObject(viewModel);
                return RedirectToAction("Deactivate", viewModel);
            }


            await _mojMinisterComponent.UpdateMoJMinisterAsync(viewModel.ToModel());
            return RedirectToAction("index");
        }

        public ActionResult inactiveMoJMinisterError(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;

            return View();
        }

        public ActionResult Deactivate(MoJMinisterViewModel viewmodel, bool x = false)
        {
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(MoJMinisterViewModel viewModel)
        {
            viewModel.Active = false;
            viewModel.Deactivated = DateTime.Now;
            var user = await _userComponent.GetUserByNameAsync(User.Identity!.Name);
            viewModel.DeactivatedBy = user.DisplayName;

            await _mojMinisterComponent.UpdateMoJMinisterAsync(viewModel.ToModel());

            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetActiveTrue(int id)
        {
            var mojMinister = await _mojMinisterComponent.GetMoJMinisterAsync(id);
            if (mojMinister != null)
            {
                var model = new MoJMinisterModel
                {
                    MoJMinisterId = mojMinister.MoJMinisterId,
                    Name = mojMinister.Name,
                    Prefix = mojMinister.Prefix,
                    Suffix = mojMinister.Suffix,
                    Rank = mojMinister.Rank,
                    Deactivated = mojMinister.Deactivated,
                    DeactivatedBy = mojMinister.DeactivatedBy,
                    Active = true
                };

                await _mojMinisterComponent.UpdateMoJMinisterAsync(model);
            }
            return Json(new { success = true });
        }
    }

}
