using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Common.AutomapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDisplayModel>()
                .ForMember(dest => dest.Authors,
                opt => opt.MapFrom(src => string.Join(", ", src.BooksAuthors.Select(x => x.Author.Name))))
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Img));
        }
    }
}
