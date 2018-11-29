using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Categories;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BookStoreContext db;
        private readonly IMapper mapper;

        public CategoryService(BookStoreContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public void Create(string categoryName)
        {
            if (!this.db.Categories.Any(c => c.Name == categoryName))
            {
                this.db.Categories.Add(new Category
                {
                    Name = categoryName
                });

                this.db.SaveChanges();
            }
        }

        public bool Edit(int id, string categoryName)
        {
            var category = this.db.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            category.Name = categoryName;

            this.db.SaveChanges();

            return true;
        }

        public bool IfCategoryExists(string categoryName)
        {
            if (this.db.Categories.Any(x => x.Name == categoryName))
            {
                return true;
            }

            return false;
        }

        public CategoryNameModel[] AllCategories()
        {
            return this.db.Categories.ProjectTo<CategoryNameModel>().ToArray();
        }

        public CategoryNameAndIdModel GetCategoryByName(string categoryName)
        {
            var currentCategory = mapper.Map<CategoryNameAndIdModel>(this.db.Categories.FirstOrDefault(x => x.Name.ToLower() == categoryName.ToLower()));

            return currentCategory;
        }
    }
}