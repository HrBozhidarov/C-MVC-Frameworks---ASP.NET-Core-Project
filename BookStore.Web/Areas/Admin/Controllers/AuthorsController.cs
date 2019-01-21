using BookStore.Common;
using BookStore.Models.ViewModels.Authors;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class AuthorsController : AdminController
    {
        private const string CreateErrorMessage = "This author name exists.";
        private const string EditErrorMessage = "This author id is invalid.";
        private const string SmallTitle = "Authors";
        private const string CollectionName = "authors";
        private const string UrlAuthorEditKendo = "/api/ApiAuthors/GetAuthors";
        private const string EventFunction = "ChooseCategory";
        private const string TextFieldAndValue = "Name";
        private const string IdElementEditAuthor = "eidtAuthor";
        private const string ViewComponentNameForEdit = "EditAuthor";

        private readonly IImgService imgService;
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;
        private readonly IHostingEnvironment environment;
        private readonly IModelKendoService modelKendoService;

        public AuthorsController(IImgService imgService, 
            IAuthorService authorService, 
            IBookService bookService,
            IHostingEnvironment environment,
            IModelKendoService modelKendoService)
        {
            this.imgService = imgService;
            this.authorService = authorService;
            this.bookService = bookService;
            this.environment = environment;
            this.modelKendoService = modelKendoService;
        }

        public IActionResult Edit()
        {
            var model = this.modelKendoService.GetModel(
                SmallTitle,
                CollectionName,
                TextFieldAndValue,
                UrlAuthorEditKendo,
                EventFunction,
                IdElementEditAuthor);

            return View(model);
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Edit(EditAuthorModel model)
        {
            string imgUrl = null;
            string path = null;
            var ifNeedToCreateImg = false;

            if (model.Image != null)
            {
                if (!this.IfImgIsValid(model.Image))
                {
                    return View(model);
                }

                var fileName = model.Image.FileName;

                var extention = this.imgService.ReturnExtension(fileName);

                var currentNumberOnFolder = this.bookService.CountOfAllBooks() % GlobalConstants.NumberOnFolder;

                var currentFoledName = GlobalConstants.FolderNameImg + currentNumberOnFolder;

                path = this.imgService.ReturnPath(environment, currentFoledName);

                imgUrl = this.imgService.ReturnUrl(environment, currentFoledName, extention);

                ifNeedToCreateImg = true;
            }

            if (!this.authorService.Edit(model.Id, model.Name, model.Details, imgUrl))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = EditErrorMessage;

                return RedirectToAction(nameof(Edit));
            }

            if (ifNeedToCreateImg)
            {
                this.imgService.CreateImg(model.Image, path, imgUrl);
            }

            return Redirect(GlobalConstants.IndexPath);
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Create(CreateAuthorModel model)
        {
            if (!this.IfImgIsValid(model.Image))
            {
                return View(model);
            }

            var fileName = model.Image.FileName;

            var extention = this.imgService.ReturnExtension(fileName);

            var currentNumberOnFolder = this.bookService.CountOfAllBooks() % GlobalConstants.NumberOnFolder;

            var currentFoledName = GlobalConstants.FolderNameImg + currentNumberOnFolder;

            var path = this.imgService.ReturnPath(environment, currentFoledName);

            var imgUrl = this.imgService.ReturnUrl(environment, currentFoledName, extention);

            if (!this.authorService.Create(model.Name, model.Details, imgUrl))
            {
                ModelState.AddModelError(string.Empty, CreateErrorMessage);
                return View();
            }

            this.imgService.CreateImg(model.Image, path, imgUrl);

            return Redirect(GlobalConstants.IndexPath);
        }

        public IActionResult EditAuthor(string dataName)
        {
            return ViewComponent(ViewComponentNameForEdit, dataName);
        }

        private bool IfImgIsValid(IFormFile img)
        {
            if (!this.imgService.CheckImgExtention(img))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = GlobalConstants.ErrorMessageExtention;

                return false;
            }

            if (!this.imgService.MingLengthOnImg(img))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = GlobalConstants.ErrorMessageMinSizeImg;

                return false;
            }

            return true;
        }
    }
}
