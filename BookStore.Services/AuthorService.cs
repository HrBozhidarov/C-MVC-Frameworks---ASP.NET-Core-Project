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

        public DetailsAuthorModel GetAuthourByNameDetails(string username)
        {
            var currentUserName = username;

            if (username.Contains("_"))
            {
                currentUserName = string.Join(" ", username.Split('_'));
            }

            var author = this.db.Authors.FirstOrDefault(x => x.Name == currentUserName);

            if (author == null)
            {
                return null;
            }

            return mapper.Map<DetailsAuthorModel>(author);
        }

        public bool Create(string authorName, string details, string imgUrl)
        {
            if (!this.db.Authors.Any(a => a.Name == authorName))
            {
                var imgPathAndName = imgUrl.Split(@"\", StringSplitOptions.RemoveEmptyEntries);
                var imgPath = $"images/{imgPathAndName[imgPathAndName.Length - 2]}/{imgPathAndName[imgPathAndName.Length - 1]}";

                this.db.Authors.Add(new Author
                {
                    Name = authorName,
                    Details = details,
                    ImgUrl = imgPath
                });

                this.db.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Edit(int id, string authorName, string details, string imgUrl)
        {
            var author = this.db.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
            {
                return false;
            }

            var imgPath = author.ImgUrl;

            if (imgUrl != null)
            {
                var imgPathAndName = imgUrl.Split(@"\", StringSplitOptions.RemoveEmptyEntries);
                imgPath = $"images/{imgPathAndName[imgPathAndName.Length - 2]}/{imgPathAndName[imgPathAndName.Length - 1]}";
            }

            author.Name = authorName;
            author.Details = details;
            author.ImgUrl = imgPath;

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
            var currentCategory = mapper.Map<EditAuthorModel>(this.db.Authors.FirstOrDefault(x => x.Name.ToLower() == authorName.ToLower()));

            return currentCategory;
        }

    }
}
