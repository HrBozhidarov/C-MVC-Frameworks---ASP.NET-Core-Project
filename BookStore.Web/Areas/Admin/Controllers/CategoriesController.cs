using BookStore.Common;
using BookStore.Models.ViewModels.Categories;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class CategoriesController : AdminController
    {
        private const string EditErrorMessage = "Yours id is invalid.";
        private const string CreateErrorMessage = "Name of the category exists.";
        private const string SmallTitle = "Categories";
        private const string CollectionName = "categories";
        private const string UrlCategoryEditKendo = "/api/ApiCategories/GetCategories";
        private const string EventFunction = "ChooseCategory";
        private const string TextFieldAndValue = "Name";
        private const string IdElementEditCategory = "editCategory";
        private const string ViewComponentNameForEdit = "EditCategory";

        private readonly ICategoryService categoryService;
        private readonly IModelKendoService modelKendoService;

        public CategoriesController(ICategoryService categoryService, IModelKendoService modelKendoService)
        {
            this.categoryService = categoryService;
            this.modelKendoService = modelKendoService;
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
                ModelState.AddModelError(string.Empty, CreateErrorMessage);

                return View(model);
            }

            return Redirect(GlobalConstants.IndexPath);
        }

        public IActionResult Edit()
        {
            var model = this.modelKendoService.GetModel(
                SmallTitle,
                CollectionName,
                TextFieldAndValue,
                UrlCategoryEditKendo,
                EventFunction,
                IdElementEditCategory);

            return View(model);
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Edit(CategoryNameAndIdModel model)
        {
            if (!this.categoryService.Edit(model.Id, model.Name))
            {
                this.TempData[GlobalConstants.TempDateErrorKey] = EditErrorMessage;

                return RedirectToAction(nameof(Edit));
            }

            return Redirect(GlobalConstants.IndexPath);
        }

        public IActionResult EditCategory(string dataName)
        {
            return ViewComponent(ViewComponentNameForEdit, dataName);
        }
    }
}
