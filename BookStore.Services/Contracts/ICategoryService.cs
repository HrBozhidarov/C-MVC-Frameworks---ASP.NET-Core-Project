using BookStore.Models.ViewModels.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface ICategoryService
    {
        bool Create(string categoryName);

        bool IfCategoryExists(string categoryName);

        CategoryNameModel[] AllCategories();

        CategoryNameAndIdModel GetCategoryByName(string categoryName);

        bool Edit(int id, string categoryName);
    }
}
