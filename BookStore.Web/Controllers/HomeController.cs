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
using BookStore.Models.ViewModels.Contacts;

namespace BookStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly IQuestionService questionService;

        public HomeController(IBookService bookService, IQuestionService questionService)
        {
            this.bookService = bookService;
            this.questionService = questionService;
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
            var model = new CreateEditContactModel();

            if (this.User.Identity.IsAuthenticated)
            {
                model = this.questionService.GetCreateEditContactModel(this.User.Identity.Name);
            }
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Contact(CreateEditContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.questionService.CreateQuestion(model.Email, model.Content, model.Title);
            this.TempData["successQuestion"] = "You have made success request, we will conncet with you!";

            return Redirect("/");
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
