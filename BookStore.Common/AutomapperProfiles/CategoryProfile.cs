using AutoMapper;
using System.Linq;
using BookStore.Models;
using BookStore.Models.ViewModels.Categories;

namespace BookStore.Common.AutomapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryNameModel>();
            CreateMap<Category, CategoryNameAndIdModel>();
        }
    }
}
