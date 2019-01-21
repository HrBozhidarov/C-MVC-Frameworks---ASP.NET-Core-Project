using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
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
        private const string GetAllAuthorBooksName = "getallauthorbooks/{name}";
        private const string GetOnlyEightBooksInDescOrderName = "desc";
        private const string GetOnlyEightBooksInAscOrderName = "asc";
        private const string GetOnlyEightBooksInAscOrderWithouthCurrentName = "ascWithouthCurrent";
        private const string GetBooksName = "GetBooks";

        private readonly IBookService bookService;

        public ApiBooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet(GetAllAuthorBooksName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookDisplayModel>> Getallauthorbooks(string name)
        {
            var books = this.bookService.GetAllBooksByAuthorName(name);

            if (books == null)
            {
                return NotFound();
            }

            return books;
        }

        [HttpGet(GetOnlyEightBooksInDescOrderName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookDisplayModel>> GetOnlyEightBooksInDescOrder()
        {
            var books = this.bookService.GetBooksInDescOrderByDate(NumberOfBooksToTake);

            if (books.Length == 0)
            {
                return NotFound();
            }

            return books;
        }

        [HttpGet(GetOnlyEightBooksInAscOrderName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BookDisplayModel>> GetOnlyEightBooksInAscOrder()
        {
            var books = this.bookService.GetBooksInAscOrderByDate(NumberOfBooksToTake);

            if (books.Length == 0)
            {
                return NotFound();
            }

            return books;
        }

        [HttpGet(GetOnlyEightBooksInAscOrderWithouthCurrentName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet(GetBooksName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string[]> GetBooks(string text)
        {
            var books = this.bookService.GetAllBooks().Select(x => x.Title).ToArray();

            if (!string.IsNullOrEmpty(text?.ToLower()))
            {
                books = books.Where(b => b.ToLower().Contains(text?.ToLower())).ToArray();
            }

            return books;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
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