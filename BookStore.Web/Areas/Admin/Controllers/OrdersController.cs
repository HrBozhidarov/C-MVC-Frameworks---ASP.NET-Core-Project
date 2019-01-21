using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class OrdersController : AdminController
    {
        private const string AllOrdersPartialName = "_GetAllOrdesPartial";

        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public PartialViewResult GetOrders(int pageIndex, int pageSize)
        {
            var orders = this.orderService.GetAllHistory()
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToArray();

            return PartialView(AllOrdersPartialName, orders);
        }

        public IActionResult AllOrdersHistory()
        {
            return View();
        }
    }
}
