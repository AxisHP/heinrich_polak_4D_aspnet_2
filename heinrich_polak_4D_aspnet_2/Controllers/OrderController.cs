using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;
using heinrich_polak_4D_aspnet_2.Models;
using Common.Enums;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private bool IsUserAdmin()
        {
            var roleString = HttpContext.Session.GetString("UserRole");
            return roleString == UserRole.Admin.ToString();
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"));
        }

        private Guid? GetCurrentUserPublicId()
        {
            var userIdString = HttpContext.Session.GetString("UserPublicId");
            if (Guid.TryParse(userIdString, out var userId))
                return userId;
            return null;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Home");

            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            var orders = IsUserAdmin() 
                ? await _orderService.GetAllAsync()
                : await _orderService.GetByUserAsync(userId.Value);

            return View(orders);
        }

        public async Task<IActionResult> Details(Guid publicId)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Home");

            var order = await _orderService.GetByPublicIdAsync(publicId);
            if (order == null) return NotFound();

            var userId = GetCurrentUserPublicId();
            if (!IsUserAdmin() && order.UserPublicId != userId)
                return RedirectToAction(nameof(Index));

            var model = new OrderDetailsModel
            {
                PublicId = order.PublicId,
                UserName = order.UserName,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Items = order.Items
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            var order = await _orderService.GetByPublicIdAsync(publicId);
            if (order == null) return NotFound();

            var model = new UpdateOrderStatusModel
            {
                PublicId = order.PublicId,
                Status = order.Status
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusModel model)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            await _orderService.UpdateStatusAsync(model.PublicId, model.Status);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            await _orderService.CancelAsync(publicId);
            return RedirectToAction(nameof(Index));
        }
    }
}
