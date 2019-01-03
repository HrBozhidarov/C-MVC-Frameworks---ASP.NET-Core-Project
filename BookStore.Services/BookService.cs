using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private const string AllCategoriesName = "all";

        private readonly BookStoreContext db;
        private readonly IMapper mapper;

        public BookService(BookStoreContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public bool Create(
            string title,
            decimal price,
            string isbn,
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
                Isbn = isbn,
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

        public DetailsBookModel GetDetailsBookById(int id)
        {
            if (!this.db.Books.Any(x => x.Id == id))
            {
                return null;
            }

            var book = this.db.Books.Include(x => x.BooksAuthors).ThenInclude(x => x.Author).Include(x => x.BooksCategories).ThenInclude(x => x.Category).First(x => x.Id == id);

            return mapper.Map<DetailsBookModel>(book);
        }

        public int CountOfAllBooks()
        {
            return this.db.Books.ToArray().Length;
        }

        public BookDisplayModel[] GetAllBooks()
        {
            return this.db.Books.ProjectTo<BookDisplayModel>().ToArray();
        }

        public bool IfIsbnExists(string isbn)
        {
            if (isbn == null)
            {
                return false;
            }

            if (this.db.Books.Any(x => x.Isbn == isbn))
            {
                return false;
            }

            return true;
        }

        public BookDisplayModel[] GetBooksInDescOrderByDate(int numberOfBooks)
        {
            return this.db.Books.OrderByDescending(x => x.ReleaseDate).Take(numberOfBooks).ProjectTo<BookDisplayModel>().ToArray();
        }

        public BookDisplayModel[] GetBooksInAscOrderByDate(int numberOfBooks)
        {
            return this.db.Books.OrderBy(x => x.ReleaseDate).Take(numberOfBooks).ProjectTo<BookDisplayModel>().ToArray();
        }

        public BookDisplayModel[] GetBooksByCategoryName(string categoryName)
        {
            if (categoryName.ToLower() == AllCategoriesName)
            {
                return this.GetAllBooks();
            }

            return this.db.BooksCategories
                .Include(x => x.Book)
                .Include(x => x.Category)
                .Where(x => x.Category.Name.ToLower() == categoryName.ToLower())
                .Select(x => x.Book)
                .ProjectTo<BookDisplayModel>()
                .ToArray();
        }

        public bool IfBookTitleContainsSearchResult(string name)
        {
            return this.db.Books.Any(x => x.Title.Contains(name));
        }

        public bool IfBookExists(int id)
        {
            return this.db.Books.Any(x => x.Id == id);
        }

        public BookDisplayModel[] GetBooksByNamePart(string name)
        {
            var books = this.db.Books.Where(x => x.Title.Contains(name)).ProjectTo<BookDisplayModel>().ToArray();

            if (books.Length == 0)
            {
                books = this.GetAllBooks();
            }

            return books;
        }
    }
}
