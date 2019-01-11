using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Authors;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.AutomapperProfiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, NameAuthorModel>();
            CreateMap<Author, EditAuthorModel>();
            CreateMap<Author, DetailsAuthorModel>();
        }
    }
}
