using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private const string ExtentionJpg = ".jpg";
        private const string ExtentionPng = ".png";
        private const string ErrorMessageExtention = "The file have to be with extention png or jpg.";
        private const string ErrorMessageNoCategoryOrAuthorh = "You have not introduce author or category.";
        private const string FolderName = "Img";
        private const string ParentFolder = "images";
        private const string ErrorMessageSizeImg = "Picture is too large or too small, it can be between 15000 and 64000 bytes.";

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

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult All(int? page)
        {
            var books = bookService.GetAllBooks();

            var pageNumber = page ?? 1;
            var onePageOfBooks = books.ToPagedList(pageNumber, ItemsPerPage);

            ViewBag.Books = onePageOfBooks;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit()
        {
            var model = this.GetModel(
               "Categories",
               "categories",
               "Name",
               "GetCategories",
               "Categories",
               "ChooseCategory",
               "editCategory");

            return View(model);
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
                ModelState.AddModelError("", "Isbn is invalid or exists.");

                return View(model);
            }

            var fileName = model.Image.FileName;

            var extention = Path.GetExtension(fileName);

            if (extention != ExtentionJpg && extention != ExtentionPng)
            {
                ModelState.AddModelError("", ErrorMessageExtention);

                return View(model);
            }

            if (MaxAllowableSize < model.Image.Length || model.Image.Length < MinAllowableSize)
            {
                ModelState.AddModelError("", ErrorMessageSizeImg);

                return View(model);
            }

            var currentNumberOnFolder = this.bookService.CountOfAllBooks() % NumberOnFolder;
            var currentFoledName = FolderName + currentNumberOnFolder;
            var newFileName = Guid.NewGuid().ToString() + extention;
            var path = Path.Combine(environment.WebRootPath + $@"\{ParentFolder}", currentFoledName);
            var imgUrl = path + $@"\{newFileName}";

            var isSeccess = this.bookService.Create(
                model.Title,
                model.Price,
                model.Isbn,
                imgUrl,
                model.Description,
                model.ReleaseDate,
                model.Authors,
                model.Categories);

            if (!isSeccess)
            {
                ModelState.AddModelError("", ErrorMessageNoCategoryOrAuthorh);

                return View(model);
            }

            CreateImg(model, path, imgUrl);

            return Redirect("/");
        }

        private void CreateImg(BookCreateModel model, string path, string imgUrl)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var imgBytesArray = new byte[0];

            using (var memoryStream = new MemoryStream())
            {
                model.Image.CopyTo(memoryStream);
                imgBytesArray = memoryStream.ToArray();
            }

            using (MagickImage image = new MagickImage(imgBytesArray))
            {
                MagickGeometry size = new MagickGeometry(154, 230);   

                image.Resize(size);

                image.Write(imgUrl);
            }
        }
    }
}
