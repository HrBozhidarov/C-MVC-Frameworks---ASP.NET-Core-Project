﻿using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BookStore.Web.Areas.Book.Controllers
{
    public class BooksController : BaseController
    {
        private const int ItemsPerPage = 6;
        private const int MaxAllowableSize = 65000;
        private const int MinAllowableSize = 5000;
        private const int NumberOnFolder = 20;
        private const string AllCategoriesName = "all";
        private const string NewReleaseCategoriesName = "newRelease";
        private const string ExtentionJpg = ".jpg";
        private const string ExtentionPng = ".png";
        private const string ErrorMessageExtention = "The file have to be with extention png or jpg.";
        private const string ErrorMessageNoCategoryOrAuthorh = "You have not introduce author or category.";
        private const string FolderName = "Img";
        private const string ParentFolder = "images";
        private const string ErrorMessageSizeImg = "Picture is too large or too small, it can be between 15000 and 64000 bytes.";
        private const string ErrorMessageDate = "Date is invalid!";
        private const string IsbnErrorMessage = "Isbn is invalid or exists.";
        private const string InvalidAttemptEdit = "Invalid attempt!";
        private const string InvalidAttemptDelete = "Invalid attempt! Choose book again and dele it.";

        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;
        private readonly IAuthorService authorService;
        private readonly IHostingEnvironment environment;

        public BooksController(IBookService bookService, ICategoryService categoryService, IAuthorService authorService, IHostingEnvironment environment)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.environment = environment;
        }

        public IActionResult Search(string search, int? page)
        {
            if (!this.bookService.IfBookTitleContainsSearchResult(search))
            {
                this.ViewData["search"] = "Search \"Not found!\"";
            }
            else
            {
                this.ViewData["search"] = $"Search for \"{search}\"";
            }

            var books = this.bookService.GetBooksByNamePart(search);

            var pageNumber = page ?? 1;
            var onePageOfBooks = books.ToPagedList(pageNumber, ItemsPerPage);

            ViewBag.Books = onePageOfBooks;
            return View();
        }

        public IActionResult Category(string category, int? page)
        {
            if (!this.categoryService.IfCategoryExists(category) && (category != AllCategoriesName))
            {
                return NotFound();
            }

            var books = this.GetBooksByCategory(category);

            var pageNumber = page ?? 1;
            var onePageOfBooks = books.ToPagedList(pageNumber, ItemsPerPage);

            ViewBag.Books = onePageOfBooks;
            return View();
        }

        public IActionResult NewRelease(int? page)
        {
            var countOfBooks = this.bookService.CountOfAllBooks();

            var books = this.bookService.GetBooksInDescOrderByDate(countOfBooks);

            var pageNumber = page ?? 1;
            var onePageOfBooks = books.ToPagedList(pageNumber, ItemsPerPage);

            ViewBag.Books = onePageOfBooks;
            return View();
        }

        public IActionResult Details(int id)
        {
            var book = this.bookService.GetDetailsBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult EditDeleteBook(string dataName)
        {
            return ViewComponent("EditDeleteBook", dataName);
        }

        public JsonResult GetBooks(string text)
        {
            var books = this.bookService.GetAllBooks().Select(x => x.Title).ToArray();

            if (!string.IsNullOrEmpty(text?.ToLower()))
            {
                books = books.Where(b => b.ToLower().Contains(text?.ToLower())).ToArray();
            }

            return Json(books);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit()
        {
            this.TempData["delete"] = false;

            var model = this.GetModel(
               "Books",
               "books",
               "",
               "GetBooks",
               "Books",
               "ChooseCategory",
               "editBook");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EditDeleteBookModel model)
        {
            if (!ModelState.IsValid)
            {
                this.TempData["error"] = InvalidAttemptEdit;

                return RedirectToAction(nameof(Edit));
            }

            if (!this.bookService.IfCurrentBookHaveTheSameIsbn(model.Id, model.Isbn))
            {
                if (!this.bookService.IfIsbnExists(model.Isbn))
                {
                    this.TempData["error"] = IsbnErrorMessage;

                    return RedirectToAction(nameof(Edit));
                }
            }

            if (!DateTime.TryParse(model.ReleaseDate, out var date))
            {
                this.TempData["error"] = ErrorMessageDate;

                return RedirectToAction(nameof(Edit));
            }

            if (model.Image != null)
            {
                var fileName = model.Image.FileName;

                var extention = Path.GetExtension(fileName);

                if (extention != ExtentionJpg && extention != ExtentionPng)
                {
                    this.TempData["error"] = ErrorMessageExtention;

                    return RedirectToAction(nameof(Edit));
                }

                if (MaxAllowableSize < model.Image.Length || model.Image.Length < MinAllowableSize)
                {
                    this.TempData["error"] = ErrorMessageSizeImg;

                    return RedirectToAction(nameof(Edit));
                }

                var currentNumberOnFolder = this.bookService.CountOfAllBooks() % NumberOnFolder;
                var currentFoledName = FolderName + currentNumberOnFolder;
                var newFileName = Guid.NewGuid().ToString() + extention;
                var path = Path.Combine(environment.WebRootPath + $@"\{ParentFolder}", currentFoledName);
                var imgUrl = path + $@"\{newFileName}";

                var isSuccess = this.bookService.Edit(
                    model.Id,
                    model.Title,
                    model.Price,
                    model.Isbn,
                    imgUrl,
                    model.Description,
                    date,
                    model.Authors,
                    model.Categories);

                if (!isSuccess)
                {
                    this.TempData["error"] = ErrorMessageNoCategoryOrAuthorh;

                    return RedirectToAction(nameof(Edit));
                }

                CreateImg(model.Image, path, imgUrl);
            }
            else
            {
                var isSuccess = this.bookService.Edit(
                    model.Id,
                    model.Title,
                    model.Price,
                    model.Isbn,
                    null,
                    model.Description,
                    date,
                    model.Authors,
                    model.Categories);

                if (!isSuccess)
                {
                    this.TempData["error"] = ErrorMessageNoCategoryOrAuthorh;

                    return RedirectToAction(nameof(Edit));
                }
            }

            return Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete()
        {
            this.TempData["delete"] = true;

            var model = this.GetModel(
              "Books",
              "books",
              "",
              "GetBooks",
              "Books",
              "ChooseCategory",
              "deleteBook");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(EditDeleteBookModel model)
        {
            if (!this.bookService.DeleteBookIsSuccess(model.Id))
            {
                this.TempData["error"] = InvalidAttemptDelete;

                return RedirectToAction(nameof(Delete));
            }

            return Redirect("/");
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [ValidateModelState]
        [HttpPost]
        public IActionResult Create(BookCreateModel model)
        {
            if (!this.bookService.IfIsbnExists(model.Isbn))
            {
                ModelState.AddModelError("", IsbnErrorMessage);

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);

                return View(model);
            }

            var fileName = model.Image.FileName;

            var extention = Path.GetExtension(fileName);

            if (extention != ExtentionJpg && extention != ExtentionPng)
            {
                ModelState.AddModelError("", ErrorMessageExtention);

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);

                return View(model);
            }

            if (MaxAllowableSize < model.Image.Length || model.Image.Length < MinAllowableSize)
            {
                ModelState.AddModelError("", ErrorMessageSizeImg);

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);

                return View(model);
            }

            var currentNumberOnFolder = this.bookService.CountOfAllBooks() % NumberOnFolder;
            var currentFoledName = FolderName + currentNumberOnFolder;
            var newFileName = Guid.NewGuid().ToString() + extention;
            var path = Path.Combine(environment.WebRootPath + $@"\{ParentFolder}", currentFoledName);
            var imgUrl = path + $@"\{newFileName}";

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
                ModelState.AddModelError("", ErrorMessageNoCategoryOrAuthorh);

                AddAllAuthorsAndCategoriesToModelForDropdownList(model);

                return View(model);
            }

            CreateImg(model.Image, path, imgUrl);

            return Redirect("/");
        }

        private void CreateImg(IFormFile image, string path, string imgUrl)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var imgBytesArray = new byte[0];

            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imgBytesArray = memoryStream.ToArray();
            }

            using (MagickImage magicImage = new MagickImage(imgBytesArray))
            {
                MagickGeometry size = new MagickGeometry(154, 230);

                magicImage.Resize(size);

                magicImage.Write(imgUrl);
            }
        }

        private BookDisplayModel[] GetBooksByCategory(string category)
        {
            return this.bookService.GetBooksByCategoryName(category);
        }

        private void AddAllAuthorsAndCategoriesToModelForDropdownList(dynamic model)
        {
            model.Authors = this.categoryService.AllCategories().Select(x => x.Name).ToList();
            model.Categories = this.authorService.AllAuthors().Select(x => x.Name).ToList();
        }
    }
}
