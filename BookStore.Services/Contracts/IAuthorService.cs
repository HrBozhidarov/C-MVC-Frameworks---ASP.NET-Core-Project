using BookStore.Models.ViewModels.Authors;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IAuthorService
    {
        bool Create(string authorName, string details, string imgUrl);

        bool Edit(int id, string authorName, string details, string imgUrl);

        bool IfAuthorExists(string authorName);

        NameAuthorModel[] AllAuthors();

        EditAuthorModel GetAuthorByName(string authorName);

        DetailsAuthorModel GetAuthourByNameDetails(string authorname);
    }
}
