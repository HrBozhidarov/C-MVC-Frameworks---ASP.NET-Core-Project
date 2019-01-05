using BookStore.Models;
using BookStore.Models.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface ICommentsService
    {
        void CreateComment(string username, int rating, int bookId, string content, string title);

        bool IfCurrentUserHaveCommentToCurrentBook(string username, int bookId);

        AprovelCommentModel[] AllCommentsWhichAreNotApproval();

        AprovelCommentModel[] AllCommentsWhichAreApproval();

        int CountCommentsWichAreApproval();

        int CountCommentsWichAreNotApproval();

        Comment GetCommentsById(int id);

        void ApproveComment(int id);

        void DeleteComment(Comment comment);

        double GetAvgRating(int bookId);

        AprovelCommentModel[] GetCommentsForCurrentBook(int bookId);

        bool IfCommentExists(int id);
    }
}
