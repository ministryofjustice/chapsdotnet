using System;
using ChapsDotNET.Business.Components;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models.Common;
using Microsoft.AspNetCore.Mvc;
using ChapsDotNET.Models;
using ChapsDotNET.Data.Entities;

namespace ChapsDotNET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserComponent _userComponent;

        public UsersController(IUserComponent usercomponent)
        {
            _userComponent = usercomponent;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            var model = new UsersViewModel();
            model.SortOrder = sortOrder;

            var users = await _userComponent.GetUsersAsync(sortOrder);
            model.Users = users;
            return View(model);
        }

        // todo - Create

        // todo - Edit
    }
}