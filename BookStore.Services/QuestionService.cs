using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Contacts;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class QuestionService : IQuestionService
    {
        private const int TwoHours = 2;
        private readonly BookStoreContext db;
        private readonly IMapper mapper;

        public QuestionService(BookStoreContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public bool DeleteItem(int id)
        {
            var element = this.db.Questions.FirstOrDefault(x => x.Id == id);

            if (element == null)
            {
                return false;
            }

            this.db.Questions.Remove(element);

            this.db.SaveChanges();

            return true;
        }

        public void CreateQuestion(string email, string content, string title)
        {
            this.db.Questions.Add(new Question
            {
                Content = content,
                CreatedOn = DateTime.UtcNow.AddHours(TwoHours),
                Email = email,
                Title = title
            });

            this.db.SaveChanges();
        }

        public VizualizeContactModel[] GetAll()
        {
            return this.db.Questions.ProjectTo<VizualizeContactModel>().ToArray();
        }

        public CreateEditContactModel GetCreateEditContactModel(string username)
        {
            var userEmail = this.db.Users.FirstOrDefault(x => x.UserName == username).Email;

            return new CreateEditContactModel
            {
                Email = userEmail
            };
        }

        public SenderContactDetailsModel GetSenderContactDetailsById(int id)
        {
            return this.db.Questions.Where(x => x.Id == id).ProjectTo<SenderContactDetailsModel>().FirstOrDefault();
        }

        public bool UpdateStateOnSeen(int id)
        {
            var question = this.db.Questions.FirstOrDefault(x => x.Id == id);

            if (question == null)
            {
                return false;
            }

            question.IsSeen = true;

            this.db.SaveChanges();

            return true;
        }

        public int NotVisitYetQuestionCount()
        {
            return this.db.Questions.Where(x => x.IsSeen == false).ToArray().Length;
        }
    }
}
