using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(OrderDto createOrderDto);
        Task<Order?> UpdateOrderAsync(int orderId, OrderDto updateOrderDto);
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}
