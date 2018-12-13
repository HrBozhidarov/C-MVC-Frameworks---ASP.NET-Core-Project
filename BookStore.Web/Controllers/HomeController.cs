using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Web.Models;
using BookStore.Services.Contracts;
using X.PagedList;

namespace BookStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int ItemsPerPage = 1;

        private readonly IBookService bookService;

        public HomeController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index(int? page)
        {
            var books = bookService.GetAllBooks();

            var pageNumber = page ?? 1;
            var onePageOfBooks = books.ToPagedList(pageNumber, ItemsPerPage);

            ViewBag.Books = onePageOfBooks;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
