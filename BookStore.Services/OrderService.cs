using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Orders;
using BookStore.Models.ViewModels.Shopping;
using BookStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookStoreContext db;

        public OrderService(BookStoreContext db)
        {
            this.db = db;
        }

        public void Create(string userId, IEnumerable<CartItem> cartItems, string address, string city, string phone)
        {
            var order = this.CreateOrder(userId,cartItems, address, city, phone);

            foreach (var item in cartItems)
            {
                this.db.OrdersBooks.Add(new OrderBook
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    OrderId = order.Id,
                });
            }

            this.db.SaveChanges();
        }

        private Order CreateOrder(string userId, IEnumerable<CartItem> cartItems, string address, string city, string phone)
        {
            var totalPrice = this.TotalPrice(cartItems);

            this.db.Orders.Add(new Order
            {
                Address = address,
                UserId = userId,
                TotalPrice = totalPrice,
                City = city,
                Phone = phone
            });

            this.db.SaveChanges();

            return this.db.Orders.Last();
        }

        private decimal TotalPrice(IEnumerable<CartItem> cartItems)
        {
            var totalPrice = 0m;

            foreach (var item in cartItems)
            {
                totalPrice += this.db.Books.First(x => x.Id == item.BookId).Price * item.Quantity;
            }

            return totalPrice;
        }
    }
}