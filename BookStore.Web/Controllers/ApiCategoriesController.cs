using BookStore.Models.ViewModels.Categories;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiCategoriesController : ApiController
    {
        private const string GetCategoriesName = "GetCategories";

        private readonly ICategoryService categoryService;

        public ApiCategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet(GetCategoriesName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CategoryNameModel[]> GetCategories(string text)
        {
            var categories = this.categoryService.AllCategories();

            if (!string.IsNullOrEmpty(text?.ToLower()))
            {
                categories = categories.Where(p => p.Name.ToLower().Contains(text?.ToLower())).ToArray();
            }

            return categories;
        }
    }
}
