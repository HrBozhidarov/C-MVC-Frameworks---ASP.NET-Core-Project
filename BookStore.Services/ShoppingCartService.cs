using BookStore.Models.ViewModels.Shopping;
using BookStore.Services.Contracts;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BookStore.Common.HelpersMethods;
using System.Text;

namespace BookStore.Services
{
    public class ShoppingCartManager: IShoppingCartManager
    {
        private readonly ConcurrentDictionary<string, ShoppingCart> carts;

        public ShoppingCartManager()
        {
            this.carts = new ConcurrentDictionary<string, ShoppingCart>();
        }

        public void AddToCart(string id, int bookId)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.AddToCart(bookId);
        }

        public CartItem GetCartItemByBookId(int bookId,string id)
        {
            var shoppingCart = this.GetShoppingCart(id);

            return shoppingCart.Items.FirstOrDefault(x => x.BookId == bookId);
        }

        public IEnumerable<CartItem> GetAllCartItems(string id)
        {
            var shoppingCart = this.GetShoppingCart(id);

            return new List<CartItem>(shoppingCart.Items);
        }

        public void RemoveItemFromCart(string id, int bookId)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.RemoveItemFromCart(bookId);
        }

        public void RemoveFromCart(string id,int bookId)
        {
            var shoppingCart = this.GetShoppingCart(id);

            shoppingCart.RemoveFromCart(bookId);
        }

        private ShoppingCart GetShoppingCart(string id)
            => this.carts.GetOrAdd(id, new ShoppingCart());
    }
}
