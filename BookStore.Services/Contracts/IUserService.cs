using BookStore.Models;
using BookStore.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IUserService
    {
        void EditFirsname(User user, string newFirstname);

        void EditLastname(User user, string newLastname);

        bool EditCity(string userId, string city);

        bool EditAddress(string userId, string address);

        bool IsUserIsValid(string email, string userId);

        UserInfoForOrderFormModel GetUserModelForForm(string username);
    }
}
