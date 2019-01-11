using BookStore.Models.ViewModels.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IQuestionService
    {
        void CreateQuestion(string email, string content, string title);

        CreateEditContactModel GetCreateEditContactModel(string username);

        int NotVisitYetQuestionCount();

        VizualizeContactModel[] GetAll();

        SenderContactDetailsModel GetSenderContactDetailsById(int id);

        bool UpdateStateOnSeen(int id);

        bool DeleteItem(int id);
    }
}
