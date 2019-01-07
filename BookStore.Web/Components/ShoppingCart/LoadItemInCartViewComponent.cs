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
            var cartItems = this.ItemsInCart();

            return View(cartItems.ToArray());
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
