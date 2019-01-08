using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Models.ViewModels.Shopping
{
    public class ShoppingCart
    {
        private readonly IList<CartItem> items;

        public ShoppingCart()
        {
            this.items = new List<CartItem>();
        }

        public IEnumerable<CartItem> Items => new List<CartItem>(items);

        public void AddToCart(int bookId)
        {
            var item = this.items.FirstOrDefault(x => x.BookId == bookId);

            if (item == null)
            {
                item = new CartItem
                {
                    BookId = bookId,
                    Quantity = 1
                };

                this.items.Add(item);
            }
            else
            {
                item.Quantity++;
            }
        }

        public void RemoveFromCart(int bookId)
        {
            var item = this.items.FirstOrDefault(x => x.BookId == bookId);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    this.RemoveItemFromCart(bookId);
                }
            }
        }

        public void RemoveItemFromCart(int bookId)
        {
            var item = this.items.FirstOrDefault(x => x.BookId == bookId);

            if (item != null)
            {
                this.items.Remove(item);
            }
        }

        public void Clear()
        {
            if (this.items.Count>0)
            {
                this.items.Clear();
            }
        }
    }
}
