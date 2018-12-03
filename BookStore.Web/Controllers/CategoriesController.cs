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
        private const string CreateErrorMessage = "Name of the category exists.";

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
            if (!this.categoryService.Create(model.Name))
            {
                ModelState.AddModelError("", CreateErrorMessage);

                return View(model);
            }

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

        public IActionResult EditCategory(string dataName)
        {
            return ViewComponent("EditCategory", dataName);
        }
    }
}