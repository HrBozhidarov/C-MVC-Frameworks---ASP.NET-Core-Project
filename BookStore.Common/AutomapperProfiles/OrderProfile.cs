using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Books;
using BookStore.Models.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookStore.Common.AutomapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, HistoryModel>()
                 .ForMember(dest => dest.OrderedOn,
                opt => opt.MapFrom(src => src.OrderedOn.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Books,
                opt => opt.MapFrom(src => src.OrderBooks.Where(x => x.OrderId == src.Id)
                .Select(x => new VisualizeBooktemsModel
                {
                    Id = x.BookId,
                    Price = x.Book.Price,
                    Quantity = x.Quantity,
                    Title = x.Book.Title
                })));
        }
    }
}
