using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Comments;
using BookStore.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly BookStoreContext db;

        public CommentsService(BookStoreContext db)
        {
            this.db = db;
        }

        public double GetAvgRating(int bookId)
        {
            var rattingModel = new RattingModel();

            var comments = this.db.Comments.Where(x => x.BookId == bookId && x.IsVisible).ToArray();

            foreach (var comment in comments)
            {
                var ratting = comment.Rating;

                switch (ratting)
                {
                    case 1: rattingModel.OneStartCount++; break;
                    case 2: rattingModel.TwoStartCount++; break;
                    case 3: rattingModel.ThreeStartCount++; break;
                    case 4: rattingModel.FourStartCount++; break;
                    case 5: rattingModel.FiveStartCount++; break;
                }
            }

            double upperPart = rattingModel.FiveStartCount * 5 + rattingModel.FourStartCount * 4 + rattingModel.ThreeStartCount * 3
                + rattingModel.TwoStartCount * 2 + rattingModel.OneStartCount;

            var bottomPart = rattingModel.FiveStartCount + rattingModel.FourStartCount + rattingModel.ThreeStartCount + rattingModel.TwoStartCount + rattingModel.OneStartCount;

            var avgRatingPosibleNaN = upperPart / bottomPart;

            var avgRating = double.IsNaN(avgRatingPosibleNaN) ? 0d : avgRatingPosibleNaN;

            return avgRating;
        }

        public Comment GetCommentsById(int id)
        {
            return this.db.Comments.FirstOrDefault(x => x.Id == id);
        }

        public void ApproveComment(int id)
        {
            var comment = this.db.Comments.FirstOrDefault(x => x.Id == id);
            comment.IsVisible = true;

            this.db.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            this.db.Comments.Remove(comment);
            this.db.SaveChanges();
        }

        public void CreateComment(string username, int rating, int bookId, string content, string title)
        {
            var userId = this.db.Users.FirstOrDefault(x => x.UserName == username).Id;

            var comment = new Comment
            {
                UserId = userId,
                BookId = bookId,
                Rating = rating,
                Content = content,
                Title = title,
                PostedOn = DateTime.Now
            };

            this.db.Comments.Add(comment);

            this.db.SaveChanges();
        }

        public bool IfCurrentUserHaveCommentToCurrentBook(string username, int bookId)
        {
            var userId = this.db.Users.FirstOrDefault(x => x.UserName == username).Id;

            return this.db.Comments.Any(x => x.UserId == userId && x.BookId == bookId);
        }

        public AprovelCommentModel[] GetCommentsForCurrentBook(int bookId)
        {
            return this.db.Comments.Where(x => x.BookId == bookId && x.IsVisible == true).ProjectTo<AprovelCommentModel>().ToArray();
        }

        public AprovelCommentModel[] AllCommentsWhichAreNotApproval()
        {
            return GetComments(false);
        }

        public AprovelCommentModel[] AllCommentsWhichAreApproval()
        {
            return GetComments(true);
        }

        public int CountCommentsWichAreApproval()
        {
            return CountComments(true);
        }

        public int CountCommentsWichAreNotApproval()
        {
            return CountComments(false);
        }

        private int CountComments(bool isVisible)
        {
            return this.db.Comments.Where(x => x.IsVisible == isVisible).Count();
        }

        private AprovelCommentModel[] GetComments(bool isVisible)
        {
            var b = this.db.Comments
               .Include(x => x.User)
               .Include(x => x.Book)
               .Where(x => x.IsVisible == isVisible)
               .ProjectTo<AprovelCommentModel>().ToArray();

            return this.db.Comments
               .Include(x => x.User)
               .Include(x => x.Book)
               .Where(x => x.IsVisible == isVisible)
               .ProjectTo<AprovelCommentModel>().ToArray();
        }
    }
}
