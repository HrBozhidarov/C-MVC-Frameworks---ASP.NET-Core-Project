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
        private const string CreateErrorMessage = "This author name exists.";
        private const string EditErrorMessage = "This author id is invalid.";

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
                ModelState.AddModelError("", EditErrorMessage);
                return RedirectToAction(nameof(Edit));
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
            if (!this.authorService.Create(model.Name, model.Details))
            {
                ModelState.AddModelError("", CreateErrorMessage);
                return View();
            }

            return Redirect("/");
        }

        public IActionResult EditAuthor(string dataName)
        {
            return ViewComponent("EditAuthor", dataName);
        }
    }
}
