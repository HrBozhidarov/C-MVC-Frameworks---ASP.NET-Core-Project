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

namespace BookStore.Web.Controllers
{
    public class CommentsController : BaseController
    {
        private const string RedirectToLoginPath = "/Identity/Account/Login";
        private const string RedirectToCurrentBookDetails = "/books/books/details";

        private readonly ICommentsService commentsService;
        private readonly IBookService bookService;
        private readonly UserManager<User> userManager;

        public CommentsController(ICommentsService commentsService, IBookService bookService, UserManager<User> userManager)
        {
            this.commentsService = commentsService;
            this.bookService = bookService;
            this.userManager = userManager;
        }

        [HttpPost]
        public IActionResult Create(CreateCommentModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                this.TempData.Put("model", model);

                return Redirect(RedirectToLoginPath);
            }

            if (!ModelState.IsValid || !this.bookService.IfBookExists(model.BookId))
            {
                return BadRequest();
            }

            var username = this.User.Identity.Name;

            if (this.commentsService.IfCurrentUserHaveCommentToCurrentBook(username, model.BookId))
            {
                return Redirect("/");
            }

            this.commentsService.CreateComment(username, model.Rating, model.BookId, model.Content, model.Title);

            return Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ApprovalComments()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id, int bookId)
        {
            var comment = this.commentsService.GetCommentsById(id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.BookId != bookId)
            {
                return NotFound();
            }

            this.commentsService.ApproveComment(id);

            return Redirect($"{RedirectToCurrentBookDetails}/{bookId}");
        }

        public IActionResult Edit(int id,int bookId)
        {
            if (!this.commentsService.IfCommentExists(id))
            {
                return NotFound();
            }



            return Redirect($"{RedirectToCurrentBookDetails}/{bookId}");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id, int bookId = 0)
        {
            var comment = this.commentsService.GetCommentsById(id);

            if (comment == null)
            {
                return NotFound();
            }

            if (bookId != 0 && comment.BookId != bookId)
            {
                return NotFound();
            }

            this.commentsService.DeleteComment(comment);

            if (bookId != 0)
            {
                return Redirect($"{RedirectToCurrentBookDetails}/{bookId}");
            }

            return RedirectToAction(nameof(ApprovalComments));
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public PartialViewResult GetDataNotApprovalComments(int pageIndex, int pageSize)
        {
            var notApprovalComments = this.commentsService
                .AllCommentsWhichAreNotApproval()
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();

            return PartialView("_GetNotApprovalCommentsPartial", notApprovalComments);
        }

        [IgnoreAntiforgeryToken]
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

            return PartialView("_GetApprovalCommentsPartial", approvalComments);
        }
    }
}
