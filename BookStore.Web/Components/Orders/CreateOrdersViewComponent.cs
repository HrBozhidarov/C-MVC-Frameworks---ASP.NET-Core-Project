using AutoMapper;
using BookStore.Common.HelpersMethods;
using BookStore.Models.ViewModels.Books;
using BookStore.Models.ViewModels.Orders;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.Orders
{
    public class CreateOrdersViewComponent : ViewComponent
    {
        private readonly IShoppingCartManager shoppingCartManager;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IBookService bookService;

        public CreateOrdersViewComponent(IShoppingCartManager shoppingCartManager, IMapper mapper, IUserService userService, IBookService bookService)
        {
            this.shoppingCartManager = shoppingCartManager;
            this.mapper = mapper;
            this.userService = userService;
            this.bookService = bookService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new OrderFormModel
            {
                UserInfo = this.userService.GetUserModelForForm(this.User.Identity.Name),
                BooksInfo = ItemsInCart()
            };

            return View(model);
        }

        private List<VisualizeBooktemsModel> ItemsInCart()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);
            var cartItems = new List<VisualizeBooktemsModel>();

            foreach (var item in items)
            {
                cartItems.Add(this.bookService.GetItemBook(item.BookId, item.Quantity));
            }

            return cartItems;
        }
    }
}
