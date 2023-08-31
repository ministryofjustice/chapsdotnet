using Microsoft.AspNetCore.Mvc;

namespace Chaps.Areas.Admin.Controllers
{
    ////[ChapsRedirect]
    //[AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
    //[Authorize]
    //[ValidateAntiForgeryTokenOnAllPosts]

    [Area("Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Lookups()
        {
            return View();
        }

    }
}


