using AutoMapper;
using BookStore.Models;
using BookStore.Models.ViewModels.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Common.AutomapperProfiles
{
    public class QuestionsProfile : Profile
    {
        public QuestionsProfile()
        {
            CreateMap<Question, VizualizeContactModel>(); 
            CreateMap<Question, SenderContactDetailsModel>();
        }
    }
}
