using System.Text.RegularExpressions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly IReportComponent _reportComponent;

        public ReportController(IReportComponent reportComponent)
        {
            _reportComponent = reportComponent;
        }

        public async Task<ActionResult> Index()
        {
            var result = await _reportComponent.GetReportsAsync();

            return View(result);
        }


        public async Task<ActionResult> Edit(int id)
        {
            var model = await _reportComponent.GetReportAsync(id);
            return View(model.ToViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ReportViewModel viewModel)
        {
            var description = viewModel.Description!;
            string noHtml = Regex.Replace(description, "<.*?>", String.Empty).Trim();

            if (String.IsNullOrWhiteSpace(noHtml))
            {
                ModelState.AddModelError("Description", "Description is required.");
            }

            if (ModelState.IsValid)
            {
                await _reportComponent.UpdateReportAsync(viewModel.ToModel());
                return RedirectToAction("index");
            }
            return View();
        }
    }
}
