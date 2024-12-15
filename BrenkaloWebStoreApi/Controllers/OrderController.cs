using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                return Ok(order);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("orders-by-user-id/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { message = "No orders found for the user." });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto createOrderDto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto updateOrderDto)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderAsync(id, updateOrderDto);

                if (updatedOrder == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            try
            {
                var isUpdated = await _orderService.UpdateOrderStatusAsync(id, newStatus);

                if (!isUpdated)
                {
                    return NotFound(new { message = "Order not found" });
                }

                return Ok(new { message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
