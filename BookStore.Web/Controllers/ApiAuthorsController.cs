using BookStore.Models.ViewModels.Authors;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiAuthorsController : ApiController
    {
        private const string GetAuthorsName = "GetAuthors";

        private readonly IAuthorService authorService;

        public ApiAuthorsController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet(GetAuthorsName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<NameAuthorModel[]> GetAuthors(string text)
        {
            var authors = this.authorService.AllAuthors();

            if (!string.IsNullOrEmpty(text?.ToLower()))
            {
                authors = authors.Where(a => a.Name.ToLower().Contains(text?.ToLower())).ToArray();
            }

            return authors;
        }
    }
}
