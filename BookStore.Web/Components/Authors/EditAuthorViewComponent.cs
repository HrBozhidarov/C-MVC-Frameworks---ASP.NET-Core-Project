using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Authors
{
    public class EditAuthorViewComponent : ViewComponent
    {
        private const string ErrorMessagge = "Author is not found.";

        private readonly IAuthorService authorService;

        public EditAuthorViewComponent(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string authorName)
        {
            if (authorName == null)
            {
                authorName = this.authorService.AllAuthors()?.FirstOrDefault()?.Name;
            }

            var author = this.authorService.GetAuthorByName(authorName);

            if (author == null)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(author);
        }
    }
}
