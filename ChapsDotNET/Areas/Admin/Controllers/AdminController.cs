using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers


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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lookups()
        {
            return View();
        }

    }
}


