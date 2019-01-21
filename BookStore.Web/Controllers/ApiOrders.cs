using BookStore.Models.ViewModels.Orders;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ApiOrders : ApiController
    {
        private const string GetDataName = "Income";
        private const string GetOrdersCountName = "countOrders";

        private readonly IOrderService orderService;

        public ApiOrders(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [IgnoreAntiforgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(GetDataName)]
        public ActionResult<IncomeModel> GetData([FromBody]MinMaxOrderDateModel model)
        {
            if (!DateTime.TryParse(model.MinDate, out var start))
            {
                return NotFound();
            }

            if (!DateTime.TryParse(model.MaxDate, out var end))
            {
                return NotFound();
            }

            var incomeModel = this.orderService.GetIncomeModel(start, end);

            if (incomeModel == null)
            {
                return NotFound();
            }

            return incomeModel;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(GetOrdersCountName)]
        public ActionResult<int> GetOrdersCount()
        {
            return this.orderService.GetAllHistory().Length;
        }
    }
}
