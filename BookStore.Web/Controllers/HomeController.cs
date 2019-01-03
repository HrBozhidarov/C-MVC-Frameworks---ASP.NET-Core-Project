using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Web.Models;
using BookStore.Services.Contracts;
using X.PagedList;
using BookStore.Models.ViewModels.Books;
using System.Threading;

namespace BookStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;

        public HomeController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
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

        //[HttpGet]
        //public JsonResult GetData(int pageIndex, int pageSize)
        //{
        //    var list = new List<BookDisplayModel>();

        //    //Thread.Sleep(2000);

        //    for (int i = 0; i < 7; i++)
        //    {
        //        var books = this.bookService.GetAllBooks();

        //        list.AddRange(books);
        //    }

        //    list=list.Skip(pageIndex * pageSize)
        //         .Take(pageSize).ToList();

        //    return Json(list);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public PartialViewResult SliderPictures([FromBody]BookDisplayModel[] books)
        {
            return PartialView("_IndexSliderPartial", books);
        }
    }
}
