using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.AutomapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfoForOrderFormModel>()
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
