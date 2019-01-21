using BookStore.Models.ViewModels.Books;
using BookStore.Models.ViewModels.Shopping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IShoppingCartManager
    {
        void AddToCart(string id, int bookId);

        void RemoveItemFromCart(string id, int bookId);

        IEnumerable<CartItem> GetAllCartItems(string id);

        CartItem GetCartItemByBookId(int bookId, string id);

        void RemoveFromCart(string id, int bookId);

        void Clear(string id);
    }
}
