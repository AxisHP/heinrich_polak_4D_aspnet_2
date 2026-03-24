using BusinessLayer.Interfaces.Services;
using Common.DTO;
using heinrich_polak_4D_aspnet_2.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace heinrich_polak_4D_aspnet_2.Controllers.Api
{
    [ApiController]
    [Route("api/admin/orders")]
    public class AdminOrdersApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrdersApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{publicId:guid}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(Guid publicId)
        {
            var order = await _orderService.GetByPublicIdAsync(publicId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPut("{publicId:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid publicId, [FromBody] ApiUpdateOrderStatusRequest request)
        {
            var updated = await _orderService.UpdateStatusAsync(publicId, request.Status);
            if (!updated)
                return NotFound();

            return Ok(new { message = "Order status updated." });
        }

        [HttpPost("{publicId:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid publicId)
        {
            var canceled = await _orderService.CancelAsync(publicId);
            if (!canceled)
                return NotFound();

            return Ok(new { message = "Order canceled." });
        }
    }
}
