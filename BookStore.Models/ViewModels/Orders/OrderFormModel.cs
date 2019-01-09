using BookStore.Models.ViewModels.Books;
using BookStore.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Orders
{
    public class OrderFormModel
    {
        public UserInfoForOrderFormModel UserInfo { get; set; }

        public IList<VisualizeBooktemsModel> BooksInfo { get; set; }
    }
}
