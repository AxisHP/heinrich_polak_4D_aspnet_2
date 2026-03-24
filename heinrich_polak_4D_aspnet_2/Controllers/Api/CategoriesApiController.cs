using BusinessLayer.Interfaces.Services;
using Common.DTO;
using heinrich_polak_4D_aspnet_2.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ApiCreateOrUpdateCategoryRequest request)
        {
            var dto = new CategoryDTO
            {
                PublicId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };

            var created = await _categoryService.CreateAsync(dto);
            if (!created)
                return BadRequest(new { message = "Could not create category." });

            return CreatedAtAction(nameof(GetCategory), new { publicId = dto.PublicId }, dto);
        }

        [HttpGet("{publicId:guid}")]
        public async Task<IActionResult> GetCategory(Guid publicId)
        {
            var category = await _categoryService.GetByPublicIdAsync(publicId);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPut("{publicId:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid publicId, [FromBody] ApiCreateOrUpdateCategoryRequest request)
        {
            var dto = new CategoryDTO
            {
                PublicId = publicId,
                Name = request.Name,
                Description = request.Description
            };

            var updated = await _categoryService.UpdateAsync(dto);
            if (!updated)
                return NotFound();

            return Ok(dto);
        }

        [HttpDelete("{publicId:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid publicId)
        {
            var deleted = await _categoryService.DeleteAsync(publicId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
