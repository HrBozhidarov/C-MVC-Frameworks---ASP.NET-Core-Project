using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Categories
{
    public class CategoriesViewComponent : ViewComponent
    {
        private const string ErrorMessagge = "No categories, you have to add.";

        private readonly ICategoryService categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = this.categoryService.AllCategories();

            if (categories.Length == 0)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(categories);
        }
    }
}
