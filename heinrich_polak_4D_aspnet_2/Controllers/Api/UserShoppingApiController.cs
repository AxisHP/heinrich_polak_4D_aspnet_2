using BusinessLayer.Interfaces.Services;
using Common.DTO;
using heinrich_polak_4D_aspnet_2.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/users/{userPublicId:guid}")]
    public class UserShoppingApiController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IFavouriteService _favouriteService;
        private readonly IOrderService _orderService;

        public UserShoppingApiController(ICartService cartService, IFavouriteService favouriteService, IOrderService orderService)
        {
            _cartService = cartService;
            _favouriteService = favouriteService;
            _orderService = orderService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetCart(Guid userPublicId)
        {
            var items = await _cartService.GetByUserAsync(userPublicId);
            var total = await _cartService.GetCartTotalAsync(userPublicId);
            var count = await _cartService.GetCartCountAsync(userPublicId);

            return Ok(new
            {
                Items = items,
                Total = total,
                Count = count
            });
        }

        [HttpPost("cart/items")]
        public async Task<IActionResult> AddToCart(Guid userPublicId, [FromBody] ApiAddToCartRequest request)
        {
            var result = await _cartService.AddAsync(userPublicId, request.ItemPublicId, request.Quantity);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("cart/items/{itemPublicId:guid}")]
        public async Task<IActionResult> UpdateCartItem(Guid userPublicId, Guid itemPublicId, [FromBody] ApiUpdateCartItemRequest request)
        {
            var result = await _cartService.UpdateQuantityAsync(userPublicId, itemPublicId, request.Quantity);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("cart/items/{itemPublicId:guid}")]
        public async Task<IActionResult> RemoveFromCart(Guid userPublicId, Guid itemPublicId)
        {
            var removed = await _cartService.RemoveAsync(userPublicId, itemPublicId);
            if (!removed)
                return NotFound(new { message = "Item is not in cart." });

            return NoContent();
        }

        [HttpDelete("cart")]
        public async Task<IActionResult> ClearCart(Guid userPublicId)
        {
            await _cartService.ClearCartAsync(userPublicId);
            return NoContent();
        }

        [HttpPost("cart/checkout")]
        public async Task<IActionResult> Checkout(Guid userPublicId)
        {
            var success = await _orderService.CreateFromCartAsync(userPublicId);
            if (!success)
                return BadRequest(new { message = "Cannot complete checkout. Some items may be out of stock or have insufficient quantity." });

            return Ok(new { message = "Checkout completed successfully." });
        }

        [HttpGet("favourites")]
        public async Task<ActionResult<List<FavouriteDTO>>> GetFavourites(Guid userPublicId)
        {
            var favourites = await _favouriteService.GetByUserAsync(userPublicId);
            return Ok(favourites);
        }

        [HttpPost("favourites/{itemPublicId:guid}")]
        public async Task<IActionResult> AddFavourite(Guid userPublicId, Guid itemPublicId)
        {
            var added = await _favouriteService.AddAsync(userPublicId, itemPublicId);
            if (!added)
                return BadRequest(new { message = "Could not add item to favourites." });

            return Ok(new { message = "Item added to favourites." });
        }

        [HttpDelete("favourites/{itemPublicId:guid}")]
        public async Task<IActionResult> RemoveFavourite(Guid userPublicId, Guid itemPublicId)
        {
            var removed = await _favouriteService.RemoveAsync(userPublicId, itemPublicId);
            if (!removed)
                return NotFound(new { message = "Item is not in favourites." });

            return NoContent();
        }

        [HttpGet("orders")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders(Guid userPublicId)
        {
            var orders = await _orderService.GetByUserAsync(userPublicId);
            return Ok(orders);
        }

        [HttpGet("orders/{orderPublicId:guid}")]
        public async Task<ActionResult<OrderDTO>> GetOrderDetails(Guid userPublicId, Guid orderPublicId)
        {
            var order = await _orderService.GetByPublicIdAsync(orderPublicId);
            if (order == null || order.UserPublicId != userPublicId)
                return NotFound();

            return Ok(order);
        }
    }
}