using AutoMapper;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Users;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class UserService : IUserService
    {
        private readonly BookStoreContext db;
        private readonly IMapper mapper;

        public UserService(BookStoreContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public UserInfoForOrderFormModel GetUserModelForForm(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            if (!this.db.Users.Any(x => x.UserName == username))
            {
                return null;
            }

            var user = this.mapper.Map<UserInfoForOrderFormModel>(this.db.Users.First(x => x.UserName == username));

            return user;
        }

        public bool IsUserIsValid(string email, string userId)
        {
            return this.db.Users.Any(x => x.Email == email && x.Id == userId);
        }

        public bool EditAddress(string userId, string address)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return false;
            }

            if (user.Address != address)
            {
                user.Address = address;

                this.db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool EditCity(string userId, string city)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return false;
            }

            if (user.City != city)
            {
                user.City = city;

                this.db.SaveChanges();

                return true;
            }

            return false;
        }

        public void EditFirsname(User user, string newFirstname)
        {
            if (user == null)
            {
                return;
            }

            user.Firstname = newFirstname;
            this.db.SaveChanges();
        }

        public void EditLastname(User user, string newLastname)
        {
            if (user == null)
            {
                return;
            }

            user.Lastname = newLastname;
            this.db.SaveChanges();
        }
    }
}
