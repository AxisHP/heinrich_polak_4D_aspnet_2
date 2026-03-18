using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogApiController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;

        public CatalogApiController(IItemService itemService, ICategoryService categoryService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
        }

        [HttpGet("items")]
        public async Task<ActionResult<List<ItemDTO>>> GetItems([FromQuery] Guid? categoryId)
        {
            var items = categoryId.HasValue
                ? await _itemService.GetByCategoryAsync(categoryId.Value)
                : await _itemService.GetAllAsync();

            return Ok(items);
        }

        [HttpGet("items/{publicId:guid}")]
        public async Task<ActionResult<ItemDTO>> GetItem(Guid publicId)
        {
            var item = await _itemService.GetByPublicIdAsync(publicId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("categories/{publicId:guid}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(Guid publicId)
        {
            var category = await _categoryService.GetByPublicIdAsync(publicId);
            if (category == null)
                return NotFound();

            return Ok(category);
        }
    }
}