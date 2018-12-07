using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Book.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : BaseController
    {
        private const string ExtentionJpg = ".jpg";
        private const string ExtentionPng = ".png";
        private const string ErrorMessageExtention = "The file have to be with extention png or jpg.";
        private const string ErrorMessageSizeImg = "Picture is too large or too small, it can be between 15000 and 64000 bytes.";
        private const string ErrorMessageNoCategoryOrAuthorh = "You have not introduce author or category.";
        private const string FolderName = "Img";
        private const string ParentFolder = "images";
        private const int MaxAllowableSize = 65000;
        private const int MinAllowableSize = 5000;
        private const int NumberOnFolder = 20;

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

            using (FileStream fs = new FileStream(imgUrl, FileMode.Create))
            {
                model.Image.CopyTo(fs);
            }
        }
    }
}
