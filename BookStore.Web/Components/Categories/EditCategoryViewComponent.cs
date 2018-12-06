using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Categories
{
    public class EditCategoryViewComponent : ViewComponent
    {
        private const string ErrorMessagge = "Category is not found.";

        private readonly ICategoryService categoryService;

        public EditCategoryViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName)
        {
            if (categoryName == null)
            {
                categoryName = this.categoryService.AllCategories()?.FirstOrDefault()?.Name;
            }

            var category = this.categoryService.GetCategoryByName(categoryName);

            if (category == null)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(category);
        }
    }
}
