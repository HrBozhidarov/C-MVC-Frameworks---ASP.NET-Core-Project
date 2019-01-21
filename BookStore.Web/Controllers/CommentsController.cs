using BookStore.Models.ViewModels.Comments;
using Microsoft.AspNetCore.Authorization;
using BookStore.Common.HelpersMethods;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using BookStore.Models;
using BookStore.Common;

namespace BookStore.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private const string ApprovalCommentsPartialName = "_GetApprovalCommentsPartial";
        private const string RedirectToLoginPath = "/Identity/Account/Login";
        private const string TempDataKeyModel = "model";

        private readonly ICommentsService commentsService;
        private readonly IBookService bookService;

        public CommentsController(ICommentsService commentsService, IBookService bookService)
        {
            this.commentsService = commentsService;
            this.bookService = bookService;
        }

        [HttpPost]
        public IActionResult Create(CreateCommentModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                this.TempData.Put(TempDataKeyModel, model);

                return Redirect(RedirectToLoginPath);
            }

            if (!ModelState.IsValid || !this.bookService.IfBookExists(model.BookId))
            {
                return BadRequest();
            }

            var username = this.User.Identity.Name;

            if (this.commentsService.IfCurrentUserHaveCommentToCurrentBook(username, model.BookId))
            {
                return Redirect(GlobalConstants.IndexPath);
            }

            this.commentsService.CreateComment(username, model.Rating, model.BookId, model.Content, model.Title);

            return Redirect(GlobalConstants.IndexPath);
        }

        [HttpPost]
        public IActionResult GetDataApprovalComments(int pageIndex, int pageSize, int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return NotFound();
            }

            var approvalComments = this.commentsService
                .GetCommentsForCurrentBook(bookId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();

            return PartialView(ApprovalCommentsPartialName, approvalComments);
        }
    }
}
