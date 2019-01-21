using BookStore.Models.ViewModels.Books;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using BookStore.Common.HelpersMethods;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Components.ShoppingCart
{
    public class LoadItemInCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartManager shoppingCartManager;
        private readonly IBookService bookService;

        public LoadItemInCartViewComponent(IShoppingCartManager shoppingCartManager, IBookService bookService)
        {
            this.shoppingCartManager = shoppingCartManager;
            this.bookService = bookService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);
            var allBooksInCurrentShoppingCart = this.bookService.GetBooksInCurrentShoppingCart(items);

            return View(allBooksInCurrentShoppingCart.ToArray());
        }
    }
}
