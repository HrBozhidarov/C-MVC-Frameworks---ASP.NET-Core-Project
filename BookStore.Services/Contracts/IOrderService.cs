using BookStore.Models.ViewModels.Orders;
using BookStore.Models.ViewModels.Shopping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IOrderService
    {
        void Create(string userId, IEnumerable<CartItem> cartItems, string address, string city, string phone);

        HistoryModel[] GetAllHistory();

        MinMaxOrderDateModel GetMinAndMaxRangeDateOnOrderBook();

        IncomeModel GetIncomeModel(DateTime start, DateTime end);
    }
}
