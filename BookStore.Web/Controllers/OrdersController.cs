using BookStore.Common.HelpersMethods;
using BookStore.Models.ViewModels.Orders;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IUserService userService;
        private readonly IShoppingCartManager shoppingCartManager;

        public OrdersController(IOrderService orderService, IUserService userService, IShoppingCartManager shoppingCartManager)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.shoppingCartManager = shoppingCartManager;
        }

        //[IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult VisualizeCreate()
        {
            return ViewComponent("CreateOrders");
        }

        [IgnoreAntiforgeryToken]
        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            if (!ModelState.IsValid ||
                !this.userService.IsUserIsValid(model.Email, model.UserId))
            {
                return Redirect("/shoppingcart/details");
            }

            var sessionKey = this.HttpContext.Session.GetShopingCartKey();
            var items = this.shoppingCartManager.GetAllCartItems(sessionKey);

            if (items.Count() <= 0)
            {
                return Redirect("/shoppingcart/details");
            }

            this.orderService.Create(
                model.UserId,
                items,
                model.Address,
                model.City,
                model.Phone
                );

            return RedirectToAction(nameof(History));
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public PartialViewResult GetOrders(int pageIndex, int pageSize)
        {
            var orders = this.orderService.GetAllHistory()
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToArray();

            return PartialView("_GetAllOrdesPartial", orders);
        }

        public IActionResult History()
        {
            var username = this.User.Identity.Name;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllOrdersHistory()
        {
            return View();
        }
    }
}
