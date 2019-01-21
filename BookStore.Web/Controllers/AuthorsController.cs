using BookStore.Models.ViewModels.Authors;
using BookStore.Services.Contracts;
using BookStore.Web.Filters.Action;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class AuthorsController : BaseController
    {
        private readonly IAuthorService authorService;

        public AuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public IActionResult Details(string name)
        {
            var author = this.authorService.GetAuthourByNameDetails(name);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
    }
}
