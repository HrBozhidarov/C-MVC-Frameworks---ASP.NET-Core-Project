using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BookStore.Common.AutomapperProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, AprovelCommentModel>().ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.ImgUrl,
                opt => opt.MapFrom(src => src.Book.Img))
                .ForMember(dest => dest.TitleBook,
                opt => opt.MapFrom(src => src.Book.Title))
                 .ForMember(dest => dest.BookId,
                opt => opt.MapFrom(src => src.Book.Id))
                  .ForMember(dest => dest.PostedOn,
                opt => opt.MapFrom(src => src.PostedOn.Value.ToString("dddd, dd yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
