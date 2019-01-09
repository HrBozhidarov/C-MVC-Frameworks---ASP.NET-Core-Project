using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Users
{
    public class UserInfoForOrderFormModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
    }
}
