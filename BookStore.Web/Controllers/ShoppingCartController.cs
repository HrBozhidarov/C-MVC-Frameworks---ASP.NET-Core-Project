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
        private readonly IShoppingCartManager shoppingCartManager;
        private readonly IBookService bookService;

        public ShoppingCartController(IShoppingCartManager shoppingCartManager, IBookService bookService)
        {
            this.shoppingCartManager = shoppingCartManager;
            this.bookService = bookService;
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public int AddToCart(int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return NumberItemsInCart();
            }

            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.AddToCart(key, bookId);

            var count = this.shoppingCartManager.GetAllCartItems(key).Sum(x => x.Quantity);

            return count;
        }

        public int NumberItemsInCart()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();

            var count = this.shoppingCartManager.GetAllCartItems(key).Sum(x => x.Quantity);

            return count;
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public ActionResult<CartItem> UpdateUp(int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return null;
            }

            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.AddToCart(key, bookId);

            return this.shoppingCartManager.GetCartItemByBookId(bookId, key);
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public ActionResult<CartItem> UpdateDown(int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return null;
            }

            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.RemoveFromCart(key, bookId);

            return this.shoppingCartManager.GetCartItemByBookId(bookId, key);
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult GetShoppingTable()
        {
            var cartItems = this.ItemsInCart();

            return PartialView("_TableItemCartPartial", cartItems.ToArray());
        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult DeleteItem(int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return null;
            }

            var key = this.HttpContext.Session.GetShopingCartKey();
            this.shoppingCartManager.RemoveItemFromCart(key, bookId);

            var cartItems = this.ItemsInCart();

            return PartialView("_TableItemCartPartial", cartItems.ToArray());
        }

        public ActionResult<decimal> TotalPrice()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);

            var totalPrice = ItemsInCart().Sum(x => x.Price * x.Quantity);

            return totalPrice;
        }

        public IActionResult Details()
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
