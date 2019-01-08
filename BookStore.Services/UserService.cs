using BookStore.Data;
using BookStore.Models;
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

        public UserService(BookStoreContext db)
        {
            this.db = db;
        }

        public bool EditAddress(string userId,string address)
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

        public bool EditCity(string userId,string city)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Id == userId);
            if (user==null)
            {
                return false;
            }

            if (user.City!=city)
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
