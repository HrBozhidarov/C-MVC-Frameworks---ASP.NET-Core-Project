using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiCommentsController : ApiController
    {
        private const string GetCountOfTheCommentsWhichAreNotApprovalName = "countNotAprovel";
        private const string GetCountOfTheCommentsWhichAreApprovalName = "countAprovel";

        private readonly ICommentsService commentsService;

        public ApiCommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpGet(GetCountOfTheCommentsWhichAreNotApprovalName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> GetCountOfTheCommentsWhichAreNotApproval()
        {
            var count = this.commentsService.CountCommentsWichAreNotApproval();

            return count;
        }

        [HttpGet(GetCountOfTheCommentsWhichAreApprovalName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> GetCountOfTheCommentsWhichAreApproval()
        {
            var count = this.commentsService.CountCommentsWichAreApproval();

            return count;
        }
    }
}
