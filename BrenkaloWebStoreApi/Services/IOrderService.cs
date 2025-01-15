using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<OrderDto> CreateOrderAsync(OrderDto createOrderDto);
        Task<Order?> UpdateOrderAsync(int orderId, OrderDto updateOrderDto);
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}
