using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreContext db;

        public BookService(BookStoreContext db)
        {
            this.db = db;
        }

        public bool Create(
            string title,
            decimal price,
            string imgUrl,
            string description,
            DateTime releaseDate,
            IList<string> authors,
            IList<string> categories)
        {
            if (authors.Count == 0 || categories.Count == 0)
            {
                return false;
            }

            if (!this.db.Authors.Any(x => authors.Contains(x.Name)) || !this.db.Categories.Any(x => categories.Contains(x.Name)))
            {
                return false;
            }

            var imgPathAndName = imgUrl.Split(@"\", StringSplitOptions.RemoveEmptyEntries);
            var imgPath = $"images/{imgPathAndName[imgPathAndName.Length - 2]}/{imgPathAndName[imgPathAndName.Length - 1]}";

            var book = new Book
            {
                Description = description,
                Title = title,
                Price = price,
                Img = imgPath,
                ReleaseDate = releaseDate
            };

            this.db.Books.Add(book);

            this.db.SaveChanges();

            foreach (var author in authors)
            {
                var currentAuthorId = this.db.Authors.FirstOrDefault(x => x.Name == author).Id;

                book.BooksAuthors.Add(new BookAuthor
                {
                    AuthorId = currentAuthorId,
                    BookId = book.Id
                });
            }

            foreach (var category in categories)
            {
                var currentCategoryId = this.db.Categories.FirstOrDefault(x => x.Name == category).Id;

                book.BooksCategories.Add(new BookCategory
                {
                    CategoryId = currentCategoryId,
                    BookId = book.Id
                });
            }

            this.db.SaveChanges();

            return true;
        }

        public int CountOfAllBooks()
        {
            return this.db.Books.ToArray().Length;
        }

        public BookDisplayModel[] GetAllBooks()
        {
            return this.db.Books.ProjectTo<BookDisplayModel>().ToArray();
        }
    }
}
