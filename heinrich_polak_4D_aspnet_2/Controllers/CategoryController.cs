using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using heinrich_polak_4D_aspnet_2.Models;
using Common.Enums;

namespace heinrich_polak_4D_aspnet_2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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

        public async Task<IActionResult> Index()
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            return View(new CreateCategoryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryModel model)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            var dto = new CategoryDTO
            {
                Name = model.Name,
                Description = model.Description
            };

            await _categoryService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            var category = await _categoryService.GetByPublicIdAsync(publicId);
            if (category == null) return NotFound();

            var model = new UpdateCategoryModel
            {
                PublicId = category.PublicId,
                Name = category.Name,
                Description = category.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryModel model)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            var dto = new CategoryDTO
            {
                PublicId = model.PublicId,
                Name = model.Name,
                Description = model.Description
            };

            await _categoryService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            var category = await _categoryService.GetByPublicIdAsync(publicId);
            if (category == null) return NotFound();

            var model = new DeleteCategoryModel
            {
                PublicId = category.PublicId,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid publicId)
        {
            if (!IsUserLoggedIn() || !IsUserAdmin())
                return RedirectToAction("Index", "Home");

            await _categoryService.DeleteAsync(publicId);
            return RedirectToAction(nameof(Index));
        }
    }
}
