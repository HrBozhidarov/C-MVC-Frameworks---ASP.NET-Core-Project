using BookStore.Common.HelpersMethods;
using BookStore.Models;
using BookStore.Models.ViewModels.Comments;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class CommentsController : AdminController
    {
        private const string NotApprovalCommentsPartialName = "_GetNotApprovalCommentsPartial";
        private const string RedirectToCurrentBookDetails = "/books/details";

        private readonly ICommentsService commentsService;
        private readonly IBookService bookService;

        public CommentsController(ICommentsService commentsService, IBookService bookService)
        {
            this.commentsService = commentsService;
            this.bookService = bookService;
        }

        public IActionResult ApprovalComments()
        {
            return View();
        }

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

        public IActionResult Edit(int id, int bookId)
        {
            if (!this.commentsService.IfCommentExists(id))
            {
                return NotFound();
            }

            return Redirect($"{RedirectToCurrentBookDetails}/{bookId}");
        }

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

        [HttpPost]
        public PartialViewResult GetDataNotApprovalComments(int pageIndex, int pageSize)
        {
            var notApprovalComments = this.commentsService
                .AllCommentsWhichAreNotApproval()
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();

            return PartialView(NotApprovalCommentsPartialName, notApprovalComments);
        }
    }
}
