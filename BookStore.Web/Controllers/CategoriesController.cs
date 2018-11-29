using BookStore.Models.ViewModels.Categories;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private const string EditErrorMessage = "Yours id is invalid.";

        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Create(CategoryNameModel model)
        {
            this.categoryService.Create(model.Name);

            return Redirect("/");
        }

        public IActionResult Edit()
        {
            return View();
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Edit(CategoryNameAndIdModel model)
        {
            if (!this.categoryService.Edit(model.Id, model.Name))
            {
                ModelState.AddModelError("", EditErrorMessage);
                return RedirectToAction("Edit");
            }

            return Redirect("/");
        }

        public IActionResult EditCategory(string category)
        {
            return ViewComponent("EditCategory", category);
        }
    }
}