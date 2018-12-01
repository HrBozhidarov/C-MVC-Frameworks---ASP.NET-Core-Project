using BookStore.Models.ViewModels.Authors;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorService authorService;

        public AuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public IActionResult Edit()
        {
            return View();
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Edit(EditAuthorModel model)
        {
            if (!this.authorService.Edit(model.Id, model.Name, model.Details))
            {
                ModelState.AddModelError("", "test");
                return RedirectToAction("Edit");
            }

            return Redirect("/");
        }


        public IActionResult Create()
        {
            return View();
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Create(CreateAuthorModel model)
        {
            this.authorService.Create(model.Name, model.Details);

            return Redirect("/");
        }

        public IActionResult EditAuthor(string dataName)
        {
            return ViewComponent("EditAuthor", dataName);
        }
    }
}
