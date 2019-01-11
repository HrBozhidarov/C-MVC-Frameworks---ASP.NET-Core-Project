using BookStore.Models.ViewModels.Authors;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : BaseController
    {
        private const string CreateErrorMessage = "This author name exists.";
        private const string EditErrorMessage = "This author id is invalid.";

        private readonly IAuthorService authorService;
        private readonly IBookService bookService;
        private readonly IHostingEnvironment environment;

        public AuthorsController(IAuthorService authorService, IBookService bookService, IHostingEnvironment environment)
        {
            this.authorService = authorService;
            this.bookService = bookService;
            this.environment = environment;
        }

        [AllowAnonymous]
        public IActionResult Details(string name)
        {
            var author = this.authorService.GetAuthourByNameDetails(name);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public IActionResult Edit()
        {
            var model = this.GetModel(
                "Authors",
                "authors",
                "Name",
                "GetAuthors",
                "Authors",
                "ChooseCategory",
                "eidtAuthor");

            return View(model);
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Edit(EditAuthorModel model)
        {
            if (model.Image != null)
            {
                if (!this.CheckServerSideValidationImgCreate(model.Image))
                {
                    return RedirectToAction(nameof(Edit));
                }

                var fileName = model.Image.FileName;
                var extention = ReturnExtension(fileName);
                var currentNumberOnFolder = this.bookService.CountOfAllBooks() % NumberOnFolder;
                var currentFoledName = FolderName + currentNumberOnFolder;
                var path = base.ReturnPath(environment, currentFoledName);

                var imgUrl = base.ReturnUrl(environment, currentFoledName, extention);

                if (!this.authorService.Edit(model.Id, model.Name, model.Details, imgUrl))
                {
                    this.TempData["error"] = EditErrorMessage;
                    return RedirectToAction(nameof(Edit));
                }

                CreateImg(model.Image, path, imgUrl);
            }
            else
            {
                if (!this.authorService.Edit(model.Id, model.Name, model.Details, null))
                {
                    this.TempData["error"] = EditErrorMessage;
                    return RedirectToAction(nameof(Edit));
                }
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
            if (!base.CheckServerSideValidationImgCreate(model.Image))
            {
                return View(model);
            }

            var fileName = model.Image.FileName;
            var extention = ReturnExtension(fileName);
            var currentNumberOnFolder = this.bookService.CountOfAllBooks() % NumberOnFolder;
            var currentFoledName = FolderName + currentNumberOnFolder;
            var path = base.ReturnPath(environment, currentFoledName);

            var imgUrl = base.ReturnUrl(environment, currentFoledName, extention);

            if (!this.authorService.Create(model.Name, model.Details, imgUrl))
            {
                ModelState.AddModelError("", CreateErrorMessage);
                return View();
            }

            this.CreateImg(model.Image, path, imgUrl);

            return Redirect("/");
        }

        public IActionResult EditAuthor(string dataName)
        {
            return ViewComponent("EditAuthor", dataName);
        }

        public JsonResult GetAuthors(string text)
        {
            var authors = this.authorService.AllAuthors();

            if (!string.IsNullOrEmpty(text?.ToLower()))
            {
                authors = authors.Where(a => a.Name.ToLower().Contains(text?.ToLower())).ToArray();
            }

            return Json(authors);
        }
    }
}
