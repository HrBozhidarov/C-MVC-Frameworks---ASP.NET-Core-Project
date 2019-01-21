using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Data;
using BookStore.Models;
using BookStore.Models.ViewModels.Books;
using BookStore.Models.ViewModels.Orders;
using BookStore.Models.ViewModels.Shopping;
using BookStore.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BookStore.Services
{
    public class OrderService : IOrderService
    {
        private const int BgTimeZoneHoursPlus = 2;
        private const string DateFormatWithDash = "yyyy-MM-dd";
        private const string DateFormatSlash = "dd/MM/yyyy";

        private readonly BookStoreContext db;

        public OrderService(BookStoreContext db)
        {
            this.db = db;
        }

        public MinMaxOrderDateModel GetMinAndMaxRangeDateOnOrderBook()
        {
            var minDate = this.db.Orders.OrderBy(x => x.OrderedOn).FirstOrDefault()?.OrderedOn.ToString(DateFormatSlash, CultureInfo.InvariantCulture);
            var maxDate = this.db.Orders.OrderByDescending(x => x.OrderedOn).FirstOrDefault()?.OrderedOn.ToString(DateFormatSlash, CultureInfo.InvariantCulture);

            if (minDate == null || maxDate == null)
            {
                return null;
            }

            return new MinMaxOrderDateModel
            {
                MinDate = minDate,
                MaxDate = maxDate
            };
        }

        public HistoryModel[] GetAllHistory()
        {
            var history = this.db.Orders.ProjectTo<HistoryModel>().ToArray();

            return history;
        }

        public IncomeModel GetIncomeModel(DateTime start, DateTime end)
        {
            var minMaxOrderDateModel = this.GetMinAndMaxRangeDateOnOrderBook();

            if (!DateTime.TryParse(minMaxOrderDateModel.MinDate, out var minDate))
            {
                return null;
            }

            if (!DateTime.TryParse(minMaxOrderDateModel.MaxDate, out var maxDate))
            {
                return null;
            }

            if (start > end)
            {
                return null;
            }

            if (minDate > start)
            {
                start = minDate;
            }

            if (maxDate < end)
            {
                end = maxDate;
            }

            var income = this.db.Orders.Where(x => x.OrderedOn.Date >= start.Date && x.OrderedOn.Date <= end.Date).Sum(x => x.TotalPrice);

            return new IncomeModel
            {
                EndDate = end.ToString(DateFormatWithDash, CultureInfo.InvariantCulture),
                StartDate = start.ToString(DateFormatWithDash, CultureInfo.InvariantCulture),
                Income = income
            };
        }

        public void Create(string userId, IEnumerable<CartItem> cartItems, string address, string city, string phone)
        {
            var order = this.CreateOrder(userId, cartItems, address, city, phone);

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
                Phone = phone,
                OrderedOn = DateTime.UtcNow.AddHours(BgTimeZoneHoursPlus)
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