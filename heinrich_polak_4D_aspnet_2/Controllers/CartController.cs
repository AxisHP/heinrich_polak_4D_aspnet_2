using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;
using heinrich_polak_4D_aspnet_2.Models;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
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

            var cartItems = await _cartService.GetByUserAsync(userId.Value);
            var total = await _cartService.GetCartTotalAsync(userId.Value);

            var model = new CartViewModel
            {
                Items = cartItems,
                Total = total
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToCartModel model)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            var result = await _cartService.AddAsync(userId.Value, model.ItemPublicId, model.Quantity);
            
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Item added to cart successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(UpdateCartItemModel model)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            var result = await _cartService.UpdateQuantityAsync(userId.Value, model.ItemPublicId, model.Quantity);
            
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Cart updated successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _cartService.RemoveAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _cartService.ClearCartAsync(userId.Value);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            var result = await _orderService.CreateFromCartAsync(userId.Value);
            if (result)
                return RedirectToAction("Index", "Order");

            TempData["ErrorMessage"] = "Cannot complete checkout. Some items may be out of stock or have insufficient quantity.";
            return RedirectToAction(nameof(Index));
        }
    }
}
