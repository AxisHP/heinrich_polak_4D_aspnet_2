using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
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

            var favorites = await _favoriteService.GetByUserAsync(userId.Value);
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _favoriteService.AddAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _favoriteService.RemoveAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Index));
        }
    }
}
