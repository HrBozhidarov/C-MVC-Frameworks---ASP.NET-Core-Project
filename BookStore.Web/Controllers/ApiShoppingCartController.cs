using BookStore.Common.HelpersMethods;
using BookStore.Models.ViewModels.Shopping;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiShoppingCartController : ApiController
    {
        private const string PostAddToCartName = "AddToCart";
        private const string GetNumberItemsInCartName = "NumberItemsInCart";
        private const string PutUpdateUpName = "UpdateUp";
        private const string PutUpdateDownName = "UpdateDown";
        private const string GetTotalPriceName = "TotalPrice";
        private const string PostClearShoppingCartName = "ClearShoppingCart";

        private readonly IShoppingCartManager shoppingCartManager;
        private readonly IBookService bookService;

        public ApiShoppingCartController(IShoppingCartManager shoppingCartManager, IBookService bookService)
        {
            this.shoppingCartManager = shoppingCartManager;
            this.bookService = bookService;
        }

        [HttpPost(PostAddToCartName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> PostAddToCart([FromBody]int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return GetNumberItemsInCart();
            }

            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.AddToCart(key, bookId);

            var count = this.shoppingCartManager.GetAllCartItems(key).Sum(x => x.Quantity);

            return count;
        }

        [HttpGet(GetNumberItemsInCartName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetNumberItemsInCart()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();

            var count = this.shoppingCartManager.GetAllCartItems(key).Sum(x => x.Quantity);

            return count;
        }

        [HttpPut(PutUpdateUpName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartItem> PutUpdateUp([FromBody]int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return NotFound();
            }

            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.AddToCart(key, bookId);

            return this.shoppingCartManager.GetCartItemByBookId(bookId, key);
        }

        [HttpPut(PutUpdateDownName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartItem> PutUpdateDown([FromBody]int bookId)
        {
            if (!this.bookService.IfBookExists(bookId))
            {
                return NotFound();
            }

            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.RemoveFromCart(key, bookId);

            return this.shoppingCartManager.GetCartItemByBookId(bookId, key);
        }

        [HttpGet(GetTotalPriceName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<decimal> GetTotalPrice()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(key);
            var allBooksInCurrentShoppingCart = this.bookService.GetBooksInCurrentShoppingCart(items);

            var totalPrice = allBooksInCurrentShoppingCart.Sum(x => x.Price * x.Quantity);

            return totalPrice;
        }

        [HttpPost(PostClearShoppingCartName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> PostClearShoppingCart()
        {
            var key = this.HttpContext.Session.GetShopingCartKey();

            this.shoppingCartManager.Clear(key);

            return true;
        }
    }
}
