using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Authors;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly BookStoreContext db;

        private readonly IMapper mapper;

        public AuthorService(BookStoreContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public void Create(string authorName, string details)
        {
            if (!this.db.Authors.Any(a => a.Name == authorName))
            {
                this.db.Authors.Add(new Author
                {
                    Name = authorName,
                    Details = details
                });

                this.db.SaveChanges();
            }
        }

        public bool Edit(int id, string authorName, string details)
        {
            var author = this.db.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return false;
            }

            author.Name = authorName;
            author.Details = details;

            this.db.SaveChanges();

            return true;
        }

        public bool IfAuthorExists(string authorName)
        {
            if (this.db.Categories.Any(x => x.Name == authorName))
            {
                return true;
            }

            return false;
        }

        public NameAuthorModel[] AllAuthors()
        {
            return this.db.Authors.ProjectTo<NameAuthorModel>().ToArray();
        }

        public EditAuthorModel GetAuthorByName(string authorName)
        {
            var currentCategory = mapper.Map<EditAuthorModel>(this.db.Authors.FirstOrDefault(x =>x.Name.ToLower() == authorName.ToLower()));

            return currentCategory;
        }

    }
}
