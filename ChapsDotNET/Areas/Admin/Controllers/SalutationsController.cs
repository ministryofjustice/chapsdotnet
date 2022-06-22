using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalutationsController : Controller
    {
        private readonly ISalutationComponent _salutationComponent;

        public SalutationsController(ISalutationComponent salutationComponent)
        {
            _salutationComponent = salutationComponent;
        }

        public async Task<IActionResult> Index()
        {
            var pagedResult = await _salutationComponent
                .GetSalutationsAsync(new SalutationRequestModel
            {
                PageNumber = 2,
                PageSize = 10,
                ShowActiveAndInactive = true
            });

            return View(pagedResult.Results);
        }
    }
}
