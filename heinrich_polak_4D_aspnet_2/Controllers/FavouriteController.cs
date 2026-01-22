using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
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

            var favourites = await _favouriteService.GetByUserAsync(userId.Value);
            return View(favourites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _favouriteService.AddAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _favouriteService.RemoveAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Index));
        }
    }
}
