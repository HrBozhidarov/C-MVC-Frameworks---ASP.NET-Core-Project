using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BookStore.Web.Controllers
{
    public class BooksController : BaseController
    {
        private const int ItemsPerPage = 6;
        private const int InitialPage = 1;
        private const string AllCategoriesName = "all";
        private const string NewReleaseCategoriesName = "newRelease";
        private const string SearchNotFound = "Search \"Not found!\"";
        private const string SearchFound = "Search for \"{0}\"";
        private const string SearchKeyTempDate = "search";

        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;

        public BooksController(IBookService bookService, ICategoryService categoryService, IAuthorService authorService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }

        public IActionResult Search(string search, int? page)
        {
            if (!this.bookService.IfBookTitleContainsSearchResult(search))
            {
                this.ViewData[SearchKeyTempDate] = SearchNotFound;
            }
            else
            {
                this.ViewData[SearchKeyTempDate] = string.Format(SearchFound, search);
            }

            var books = this.bookService.GetBooksByNamePart(search);

            var pageNumber = page ?? InitialPage;
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

            var books = this.bookService.GetBooksByCategoryName(category);

            var pageNumber = page ?? InitialPage;
            var onePageOfBooks = books.ToPagedList(pageNumber, ItemsPerPage);

            ViewBag.Books = onePageOfBooks;
            return View();
        }

        public IActionResult NewRelease(int? page)
        {
            var countOfBooks = this.bookService.CountOfAllBooks();

            var books = this.bookService.GetBooksInDescOrderByDate(countOfBooks);

            var pageNumber = page ?? InitialPage;
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
    }
}
