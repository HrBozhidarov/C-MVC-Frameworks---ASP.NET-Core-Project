using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Books
{
    public class EditDeleteBookViewComponent : ViewComponent
    {
        private const string ErrorMessagge = "This book doesn`t exists.";
        private readonly IBookService bookService;

        public EditDeleteBookViewComponent(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string bookName)
        {
            if (bookName == null)
            {
                bookName = this.bookService.GetAllBooks()?.FirstOrDefault()?.Title;
            }

            var book = this.bookService.GetBookByNameForEdit(bookName);

            if (book == null)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(book);
        }
    }
}
