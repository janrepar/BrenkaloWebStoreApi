using BrenkaloWebStoreApi.Data;
using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrenkaloWebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly WebStoreContext _context;

        public OrderController(IOrderService orderService, WebStoreContext webStoreContext)
        {
            _orderService = orderService;
            _context = webStoreContext;
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
        [Authorize]
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

        [HttpGet("orders-by-user-id")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUserId([FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Unauthorized("Token is missing.");
                }

                // Remove "Bearer " prefix if present
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }

                var userSession = await _context.UserSessions
                    .FirstOrDefaultAsync(us => us.Token == token && us.ValidUntil > DateTime.UtcNow);

                if (userSession == null)
                {
                    return Unauthorized("Invalid or expired token.");
                }

                var orders = await _orderService.GetOrdersByUserIdAsync(userSession.UserId);

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
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(orderDto, token);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
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

        [HttpPatch("status/{id}")]
        [Authorize]
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
