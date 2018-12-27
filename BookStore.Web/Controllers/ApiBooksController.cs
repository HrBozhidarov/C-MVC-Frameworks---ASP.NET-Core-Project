using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiBooksController : ApiController
    {
        private const int NumberOfBooksToTake = 8;

        private readonly IBookService bookService;

        public ApiBooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("desc")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<BookDisplayModel>> GetOnlyEightBooksInDescOrder()
        {
            var books = this.bookService.GetBooksInDescOrderByDate(NumberOfBooksToTake);

            if (books.Length == 0)
            {
                return NotFound();
            }

            return books;
        }

        [HttpGet("asc")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<BookDisplayModel>> GetOnlyEightBooksInAscOrder()
        {
            var books = this.bookService.GetBooksInAscOrderByDate(NumberOfBooksToTake);

            if (books.Length == 0)
            {
                return NotFound();
            }

            return books;
        }

        [HttpGet("ascWithouthCurrent")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<BookDisplayModel>> GetOnlyEightBooksInAscOrderWithouthCurrent(int id)
        {
            var takeBooksPlusOne = NumberOfBooksToTake + 1;

            var books = this.bookService.GetBooksInAscOrderByDate(takeBooksPlusOne).Where(x => x.Id != id).ToArray();

            if (books.Length == 0)
            {
                return NotFound();
            }

            return books;
        }

        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<BookDisplayModel>> Search(string search)
        {
            var count = this.bookService.CountOfAllBooks();
            var books = this.bookService.GetBooksInAscOrderByDate(count);

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(x => x.Title.ToLower().Contains(search?.ToLower())).ToArray();
            }

            return books;
        }


    }
}