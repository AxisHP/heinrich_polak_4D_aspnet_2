using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using heinrich_polak_4D_aspnet_2.Models;
using Common.Enums;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;
        private readonly IFavoriteService _favoriteService;
        private readonly ICartService _cartService;

        public ItemController(IItemService itemService, ICategoryService categoryService, 
            IFavoriteService favoriteService, ICartService cartService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
            _favoriteService = favoriteService;
            _cartService = cartService;
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

        public async Task<IActionResult> Index(Guid? categoryId)
        {
            var items = categoryId.HasValue 
                ? await _itemService.GetByCategoryAsync(categoryId.Value)
                : await _itemService.GetAllAsync();
            
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.SelectedCategoryId = categoryId;
            return View(items);
        }

        public async Task<IActionResult> Details(Guid publicId)
        {
            var item = await _itemService.GetByPublicIdAsync(publicId);
            if (item == null) return NotFound();

            var userId = GetCurrentUserPublicId();
            if (userId.HasValue)
            {
                ViewBag.IsFavorite = await _favoriteService.IsItemFavoriteAsync(userId.Value, publicId);
            }

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "PublicId", "Name");
            return View(new CreateItemModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemModel model)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "PublicId", "Name");
                return View(model);
            }

            var dto = new ItemDTO
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId
            };

            await _itemService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            var item = await _itemService.GetByPublicIdAsync(publicId);
            if (item == null) return NotFound();

            var model = new UpdateItemModel
            {
                PublicId = item.PublicId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                StockQuantity = item.StockQuantity,
                CategoryId = item.CategoryId
            };

            ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "PublicId", "Name", item.CategoryId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateItemModel model)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryService.GetAllAsync(), "PublicId", "Name", model.CategoryId);
                return View(model);
            }

            var dto = new ItemDTO
            {
                PublicId = model.PublicId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId
            };

            await _itemService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            var item = await _itemService.GetByPublicIdAsync(publicId);
            if (item == null) return NotFound();

            var model = new DeleteItemModel
            {
                PublicId = item.PublicId,
                Name = item.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            await _itemService.DeleteAsync(publicId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _favoriteService.AddAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Details), new { publicId = itemPublicId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(Guid itemPublicId)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _favoriteService.RemoveAsync(userId.Value, itemPublicId);
            return RedirectToAction(nameof(Details), new { publicId = itemPublicId });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid itemPublicId, int quantity = 1)
        {
            var userId = GetCurrentUserPublicId();
            if (!userId.HasValue)
                return RedirectToAction("Login", "Home");

            await _cartService.AddAsync(userId.Value, itemPublicId, quantity);
            return RedirectToAction(nameof(Details), new { publicId = itemPublicId });
        }
    }
}
