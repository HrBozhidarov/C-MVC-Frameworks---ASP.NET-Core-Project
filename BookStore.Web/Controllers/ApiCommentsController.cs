using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiCommentsController : ApiController
    {
        private readonly ICommentsService commentsService;

        public ApiCommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpGet("countNotAprovel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<int> GetCountOfTheCommentsWhichAreNotApproval()
        {
            var count = this.commentsService.CountCommentsWichAreNotApproval();

            return count;
        }

        [HttpGet("countAprovel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<int> GetCountOfTheCommentsWhichAreApproval()
        {
            var count = this.commentsService.CountCommentsWichAreApproval();

            return count;
        }
    }
}
