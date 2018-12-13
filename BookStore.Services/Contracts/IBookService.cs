using BookStore.Models.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IBookService
    {
        bool Create(string title,
            decimal price,
            string imgUrl,
            string description,
            DateTime releaseDate,
            IList<string> authors,
            IList<string> categories);

        int CountOfAllBooks();

        BookDisplayModel[] GetAllBooks();
    }
}