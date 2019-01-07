using BookStore.Models.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IBookService
    {
        bool Create(
            string title,
            decimal price,
            string isbn,
            string imgUrl,
            string description,
            DateTime releaseDate,
            IList<string> authors,
            IList<string> categories);

        int CountOfAllBooks();

        BookDisplayModel[] GetAllBooks();

        bool IfIsbnExists(string isbn);

        DetailsBookModel GetDetailsBookById(int id);

        BookDisplayModel[] GetBooksInAscOrderByDate(int numberOfBooks);

        BookDisplayModel[] GetBooksInDescOrderByDate(int numberOfBooks);

        BookDisplayModel[] GetBooksByCategoryName(string categoryName);

        BookDisplayModel[] GetBooksByNamePart(string name);

        bool IfBookTitleContainsSearchResult(string name);

        bool IfBookExists(int id);

        EditDeleteBookModel GetBookByNameForEdit(string bookName);

        bool Edit(
            int id,
            string title,
            decimal price,
            string isbn,
            string imgUrl,
            string description,
            DateTime releaseDate,
            IList<string> authors,
            IList<string> categories);

        bool DeleteBookIsSuccess(int bookId);

        bool IfCurrentBookHaveTheSameIsbn(int bookId, string isbn);

        VisualizeBooktemsModel GetItemBook(int bookId, int quantity);
    }
}