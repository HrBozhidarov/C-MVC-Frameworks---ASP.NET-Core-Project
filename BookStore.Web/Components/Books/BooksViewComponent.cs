using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Books
{
    public class BooksViewComponent : ViewComponent
    {
        private const string ErrorMessagge = "No Books, you have to add.";
        private readonly IBookService bookService;

        public BooksViewComponent(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authors = this.bookService.GetAllBooks().Select(x=>x.Title).ToArray();

            if (authors.Length == 0)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(authors);
        }
    }
}
