using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Common.Mappers;
using ChapsDotNET.Frontend.Components.Breadcrumbs;
using ChapsDotNET.Frontend.Components.Button;
using ChapsDotNET.Frontend.Components.Heading;
using ChapsDotNET.Frontend.Components.Table;
using ChapsDotNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Controllers
{
    
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        //private readonly IHomeComponent _HomeComponent;

        //public HomeController(IHomeComponent HomeComponent)
        //{
        //    _HomeComponent = HomeComponent;
        //}

        //public async Task<IActionResult> Index(int page = 1)
        //{
        //    var resultsData = await _HomeComponent
        //        .GetHomeAsync(new HomeRequestModel
        //        {
        //            PageNumber = page,
        //            PageSize = 10,
        //        });

        //    var results = resultsData.Results!;
        //    var resultsCount = results.Count;

        //    // Breadcrumbs
        //    List<BreadcrumbModel> breadcrumbs = [
        //        new BreadcrumbModel { Label = "Home", Url = Url.Action("Index", "Home", new {area =""})!},
        //        new BreadcrumbModel { Label = "Admin", Url = Url.Action("Index", "Admin", new {area =""})!},
        //    ];
        //    var breadcrumbsModel = new BreadcrumbsModel(breadcrumbs);

        //    // Heading
        //    var headingModel = new HeadingModel { Title = "Public Holidays", Button = new ButtonModel { Element = ButtonModel.ValidElementTypes.anchor, Type = ButtonModel.ValidButtonStyles.secondary, Label = "Add a new public holiday", Url = Url.Action("Create", "Home") } };

        //    // Table

        //    //// Table Headers
        //    List<HeaderCell> headers = [
        //        new HeaderCell { Content = "Name", Scope = RowScopes.col},
        //        new HeaderCell { Content = "Address", Scope = RowScopes.col},
        //    ];

        //    //// Table body
        //    var rows = new List<Row>();

        //    foreach (var holiday in results!)
        //    {
        //        var row = new Row
        //        {
        //            RowContent = [
        //                new BodyCell {Content = holiday.Date.ToShortDateString(), Url = $"/Admin/Home/Edit/{holiday.HomeId}"},
        //                new BodyCell {Content = holiday.Description != null ? holiday.Description : "Not set"},
        //            ]
        //        };
        //        rows.Add(row);
        //    }

        //    var tableModel = GetTable("Existing public holidays", headers, rows, resultsData, resultsCount);

        //    // Pagination
        //    var model = new IndexViewModel
        //    {
        //        Breadcrumbs = breadcrumbsModel,
        //        Alert = GetAlerts(),
        //        Heading = headingModel,
        //        ListFilter = GetListFilter(resultsData, tableModel)
        //    };

        //    return View(model);
        //}

        //public ActionResult Create()
        //{
        //    var model = new HomeViewModel
        //    {
        //        Date = DateTime.Now.AddDays(1)
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(HomeViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var holidayId = await _HomeComponent.AddHomeAsync(viewModel.ToModel());
        //        var model = await _HomeComponent.GetHomeAsync(holidayId);
        //        TempData["alertContent"] = $"Public holiday {model.Description} created successfully";
        //        TempData["alertSummary"] = $"Public holiday created successfully";
        //        TempData["alertStatus"] = "success";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["alertContent"] = "Please enter a date in the future";
        //    TempData["alertSummary"] = $"Unable to add public holiday";
        //    TempData["alertStatus"] = "error";
        //    return View();
        //}

        //public async Task<ActionResult> Edit(int id)
        //{
        //    var model = await _HomeComponent.GetHomeAsync(id);
        //    return View(model.ToViewModel());
        //}

        //[HttpPost]
        //public async Task<ActionResult> Edit(HomeViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var holidayId = await _HomeComponent.UpdateHomeAsync(viewModel.ToModel());
        //        var model = await _HomeComponent.GetHomeAsync(holidayId);
        //        TempData["alertContent"] = $"Public holiday {model.Description} updated successfully";
        //        TempData["alertSummary"] = $"Public holiday updated successfully";
        //        TempData["alertStatus"] = "success";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["alertContent"] = "Please enter a date in the future";
        //        TempData["alertSummary"] = $"Unable to update public holiday";
        //        TempData["alertStatus"] = "error";
        //    }
        //    return View();
        //}
    }
}
