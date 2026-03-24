using BusinessLayer.Interfaces.Services;
using Common.DTO;
using heinrich_polak_4D_aspnet_2.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/items")]
    public class ItemsApiController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsApiController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ApiCreateOrUpdateItemRequest request)
        {
            var dto = new ItemDTO
            {
                PublicId = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            };

            var created = await _itemService.CreateAsync(dto);
            if (!created)
                return BadRequest(new { message = "Could not create item." });

            return CreatedAtAction(nameof(GetItem), new { publicId = dto.PublicId }, dto);
        }

        [HttpGet("{publicId:guid}")]
        public async Task<IActionResult> GetItem(Guid publicId)
        {
            var item = await _itemService.GetByPublicIdAsync(publicId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPut("{publicId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid publicId, [FromBody] ApiCreateOrUpdateItemRequest request)
        {
            var dto = new ItemDTO
            {
                PublicId = publicId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            };

            var updated = await _itemService.UpdateAsync(dto);
            if (!updated)
                return NotFound();

            return Ok(dto);
        }

        [HttpDelete("{publicId:guid}")]
        public async Task<IActionResult> DeleteItem(Guid publicId)
        {
            var deleted = await _itemService.DeleteAsync(publicId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
