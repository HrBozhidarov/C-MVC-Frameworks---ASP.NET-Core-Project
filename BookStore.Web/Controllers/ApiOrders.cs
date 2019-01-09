using BookStore.Models.ViewModels.Orders;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiOrders : ApiController
    {
        private readonly IOrderService orderService;

        public ApiOrders(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("Income")]
        public ActionResult<IncomeModel> GetData([FromBody]MinMaxOrderDateModel model)
        {
            if (!DateTime.TryParse(model.MinDate, out var start))
            {
                return null;
            }

            if (!DateTime.TryParse(model.MaxDate, out var end))
            {
                return null;
            }

            var incomeModel = this.orderService.GetIncomeModel(start, end);

            if (incomeModel == null)
            {
                return null;
            }

            return incomeModel;
        }

        [HttpGet("countOrders")]
        public ActionResult<int> GetOrdersCount()
        {
            return this.orderService.GetAllHistory().Length;
        }
    }
}
