using BookStore.Common;
using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class BooksController : AdminController
    {
        private const string ErrorMessageNoCategoryOrAuthorh = "You have not introduce author or category, choose book again and try again.";
        private const string ErrorMessageDate = "Date is invalid!";
        private const string IsbnErrorMessage = "Isbn is invalid or exists.";
        private const string InvalidAttemptEdit = "Invalid attempt!";
        private const string InvalidAttemptDelete = "Invalid attempt! Choose book again and delete it.";
        private const string TempDateDeleteKey = "delete";
        private const string SmallTitle = "Books";
        private const string CollectionName = "books";
        private const string UrlGetBooksKendo = "/api/apiBooks/getbooks";
        private const string EventFunction = "ChooseCategory";
        private const string IdElementDelete = "deleteBook";
        private const string IdElementEdit = "editBook";

        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;
        private readonly IAuthorService authorService;
        private readonly IHostingEnvironment environment;
        private readonly IImgService imgService;
        private readonly IModelKendoService modelKendoService;

        public BooksController(IBookService bookService,
            ICategoryService categoryService,
            IAuthorService authorService,
            IHostingEnvironment environment,
            IImgService imgService,
            IModelKendoService modelKendoService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.environment = environment;
            this.imgService = imgService;
            this.modelKendoService = modelKendoService;
        }

        public IActionResult Edit()
        {
            this.TempData[TempDateDeleteKey] = false;

            var model = this.modelKendoService.GetModel(
               SmallTitle,
               CollectionName,
               string.Empty,
               UrlGetBooksKendo,
               EventFunction,
               IdElementEdit);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditDeleteBookModel model)
        {
            string imgUrl = null;
            string path = null;
            var ifNeedToCreateImg = false;

            if (!ModelState.IsValid)
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = InvalidAttemptEdit;

                return RedirectToAction(nameof(Edit));
            }

            if (!this.bookService.IfCurrentBookHaveTheSameIsbn(model.Id, model.Isbn))
            {
                if (!this.bookService.IfIsbnExists(model.Isbn))
                {
                    this.TempData[GlobalConstants.TempDateErrorKey] = IsbnErrorMessage;

                    return RedirectToAction(nameof(Edit));
                }
            }

            var date = this.bookService.ConvertFromStringToDateTimeIfPossible(model.ReleaseDate);

            if (date == null)
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = ErrorMessageDate;

                return RedirectToAction(nameof(Edit));
            }

            if (model.Image != null)
            {
                if (!this.imgService.CheckImgExtention(model.Image))
                {
                    this.TempData[GlobalConstants.TempDateErrorKey] = GlobalConstants.ErrorMessageExtention;

                    return RedirectToAction(nameof(Edit));
                }

                if (!this.imgService.MingLengthOnImg(model.Image))
                {
                    this.TempData[GlobalConstants.TempDateErrorKey] = GlobalConstants.ErrorMessageMinSizeImg;

                    return RedirectToAction(nameof(Edit));
                }

                var fileName = model.Image.FileName;

                var extention = this.imgService.ReturnExtension(fileName);

                var currentNumberOnFolder = this.bookService.CountOfAllBooks() % GlobalConstants.NumberOnFolder;

                var currentFoledName = GlobalConstants.FolderNameImg + currentNumberOnFolder;

                path = this.imgService.ReturnPath(environment, currentFoledName);

                imgUrl = this.imgService.ReturnUrl(environment, currentFoledName, extention);

                ifNeedToCreateImg = true;
            }

            var isSuccess = this.bookService.Edit(
                  model.Id,
                  model.Title,
                  model.Price,
                  model.Isbn,
                  imgUrl,
                  model.Description,
                  date.Value.ToUniversalTime(),
                  model.Authors,
                  model.Categories);

            if (!isSuccess)
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = ErrorMessageNoCategoryOrAuthorh;

                return RedirectToAction(nameof(Edit));
            }

            if (ifNeedToCreateImg)
            {
                this.imgService.CreateImg(model.Image, path, imgUrl);
            }

            return Redirect(GlobalConstants.IndexPath);
        }

        public IActionResult Delete()
        {
            this.TempData[TempDateDeleteKey] = true;

            var model = this.modelKendoService.GetModel(
              SmallTitle,
              CollectionName,
              string.Empty,
              UrlGetBooksKendo,
              EventFunction,
              IdElementDelete);

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(EditDeleteBookModel model)
        {
            if (!this.bookService.DeleteBookIsSuccess(model.Id))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = InvalidAttemptDelete;

                return RedirectToAction(nameof(Delete));
            }

            return Redirect(GlobalConstants.IndexPath);
        }

        public IActionResult EditDeleteBook(string dataName)
        {
            return ViewComponent("EditDeleteBook", dataName);
        }

        public IActionResult Create()
        {
            var categories = this.categoryService.AllCategories().Select(x => x.Name).ToList();
            var authors = this.authorService.AllAuthors().Select(x => x.Name).ToList();

            var model = new BookCreateModel
            {
                Categories = categories,
                Authors = authors
            };

            return View(model);
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Create(BookCreateModel model)
        {
            if (!this.bookService.IfIsbnExists(model.Isbn))
            {
                ModelState.AddModelError(string.Empty, IsbnErrorMessage);

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);

                return View(model);
            }

            if (!this.imgService.CheckImgExtention(model.Image))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = GlobalConstants.ErrorMessageExtention;

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);
                return View(model);
            }

            if (!this.imgService.MingLengthOnImg(model.Image))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = GlobalConstants.ErrorMessageMinSizeImg;

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);
                return View(model);
            }

            var fileName = model.Image.FileName;

            var extention = this.imgService.ReturnExtension(fileName);

            var currentNumberOnFolder = this.bookService.CountOfAllBooks() % GlobalConstants.NumberOnFolder;

            var currentFoledName = GlobalConstants.FolderNameImg + currentNumberOnFolder;

            var path = this.imgService.ReturnPath(environment, currentFoledName);

            var imgUrl = this.imgService.ReturnUrl(environment, currentFoledName, extention);

            var isSuccess = this.bookService.Create(
                model.Title,
                model.Price,
                model.Isbn,
                imgUrl,
                model.Description,
                model.ReleaseDate,
                model.Authors,
                model.Categories);

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, ErrorMessageNoCategoryOrAuthorh);

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);

                return View(model);
            }

            this.imgService.CreateImg(model.Image, path, imgUrl);

            return Redirect(GlobalConstants.IndexPath);
        }

        private void AddAllAuthorsAndCategoriesToModelForDropdownList(dynamic model)
        {
            model.Authors = this.categoryService.AllCategories().Select(x => x.Name).ToList();
            model.Categories = this.authorService.AllAuthors().Select(x => x.Name).ToList();
        }
    }
}
