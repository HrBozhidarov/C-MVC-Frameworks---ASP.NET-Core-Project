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
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);
            var allBooksInCurrentShoppingCart = this.bookService.GetBooksInCurrentShoppingCart(items);

            var model = new OrderFormModel
            {
                UserInfo = this.userService.GetUserModelForForm(this.User.Identity.Name),
                BooksInfo = allBooksInCurrentShoppingCart
            };

            return View(model);
        }
    }
}
