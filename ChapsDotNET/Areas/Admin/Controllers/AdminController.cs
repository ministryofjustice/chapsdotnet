using Microsoft.AspNetCore.Mvc;

namespace Chaps.Areas.Admin.Controllers
{
    ////[ChapsRedirect]
    //[AuthorizeRedirect(MinimumRequiredAccessLevel = AccessLevel.Manager)]
    //[Authorize]
    //[ValidateAntiForgeryTokenOnAllPosts]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lookups()
        {
            return View();
        }

    }
}


