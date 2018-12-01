using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Authors
{
    public class AuthorsViewComponent : ViewComponent
    {
        private const string ErrorMessagge = "No Athors, you have to add.";
        private readonly IAuthorService authorService;

        public AuthorsViewComponent(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authors = this.authorService.AllAuthors();

            if (authors.Length == 0)
            {
                ModelState.AddModelError("", ErrorMessagge);
            }

            return View(authors);
        }
    }
}
