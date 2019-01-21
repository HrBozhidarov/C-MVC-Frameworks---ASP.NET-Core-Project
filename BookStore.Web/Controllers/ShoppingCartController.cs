using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Common.HelpersMethods;
using BookStore.Models.ViewModels.Shopping;
using BookStore.Models.ViewModels.Books;

namespace BookStore.Web.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private const string TalbeItemCartPartialName = "_TableItemCartPartial";

        private readonly IShoppingCartManager shoppingCartManager;
        private readonly IBookService bookService;

        public ShoppingCartController(IShoppingCartManager shoppingCartManager, IBookService bookService)
        {
            this.shoppingCartManager = shoppingCartManager;
            this.bookService = bookService;
        }

        [HttpPost]
        public IActionResult GetShoppingTable()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);
            var allBooksInCurrentShoppingCart = this.bookService.GetBooksInCurrentShoppingCart(items);

            return PartialView(TalbeItemCartPartialName, allBooksInCurrentShoppingCart.ToArray());
        }

        [HttpPost]
        public IActionResult DeleteItem(int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return null;
            }

            var key = this.HttpContext.Session.GetShopingCartKey();
            this.shoppingCartManager.RemoveItemFromCart(key, bookId);

            var items = this.shoppingCartManager.GetAllCartItems(key);
            var allBooksInCurrentShoppingCart = this.bookService.GetBooksInCurrentShoppingCart(items);

            return PartialView(TalbeItemCartPartialName, allBooksInCurrentShoppingCart.ToArray());
        }

        public IActionResult Details()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);
            var allBooksInCurrentShoppingCart = this.bookService.GetBooksInCurrentShoppingCart(items);

            return View(allBooksInCurrentShoppingCart.ToArray());
        }
    }
}
