using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            CreateMap<Book, DetailsBookModel>()
                .ForMember(dest => dest.Authors,
                opt => opt.MapFrom(src => src.BooksAuthors.Select(x => x.Author.Name).ToArray()))
                .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.BooksCategories.Select(x => x.Category.Name).ToArray()))
                .ForMember(dest => dest.ReleaseDate,
                opt => opt.MapFrom(src => src.ReleaseDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

            CreateMap<Book, EditDeleteBookModel>()
                .ForMember(dest => dest.Authors,
                opt => opt.MapFrom(src => src.BooksAuthors.Select(x => x.Author.Name).ToList()))
                 .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.BooksCategories.Select(x => x.Category.Name).ToList()))
                .ForMember(dest => dest.ReleaseDate,
                opt => opt.MapFrom(src => src.ReleaseDate.ToString("yyyy-MM-ddThh:mm", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.ImgUrl, opt => opt.MapFrom(src => src.Img));
        }
    }
}
