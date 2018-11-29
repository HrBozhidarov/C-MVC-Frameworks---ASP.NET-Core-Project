using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Categories
{
    public class EditCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;
        private const string ErrorMessagge = "Category is not found.";

        public EditCategoryViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName)
        {
            var category = this.categoryService.GetCategoryByName(categoryName);

            if (category == null)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(category);
        }
    }
}
