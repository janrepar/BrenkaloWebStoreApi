using BrenkaloWebStoreApi.Data;
using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BrenkaloWebStoreApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebStoreContext _context;

        public OrderService(WebStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(OrderDto createOrderDto)
        {
            var order = new Order
            {
                UserId = createOrderDto.UserId,  
                OrderShippingMethod = createOrderDto.OrderShippingMethod,
                CustomerName = createOrderDto.CustomerName,
                CustomerEmail = createOrderDto.CustomerEmail,
                CustomerPhone = createOrderDto.CustomerPhone,
                ShippingAddress = createOrderDto.ShippingAddress,
                BillingAddress = createOrderDto.BillingAddress,
                CustomerNotes = createOrderDto.CustomerNotes,
                TotalAmount = createOrderDto.TotalAmount,
                VatAmount = createOrderDto.VatAmount,
                DiscountAmount = createOrderDto.DiscountAmount,
                PaymentMethod = createOrderDto.PaymentMethod,
                OrderStatus = "PENDING"
                // CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), need to check if DB auto inserts datetime
                // UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var orderProductDto in createOrderDto.OrderProducts)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = orderProductDto.ProductId,
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == orderProductDto.ProductId)?.Name,
                    ProductSku = _context.Products.FirstOrDefault(p => p.Id == orderProductDto.ProductId)?.Sku,
                    Quantity = orderProductDto.Quantity,
                    PricePerUnit = orderProductDto.PricePerUnit,
                    TotalPrice = orderProductDto.TotalPrice,
                    VatRate = orderProductDto.VatRate,
                    VatAmount = orderProductDto.VatAmount,
                    // CreatedAt = DateTime.UtcNow,
                    // UpdatedAt = DateTime.UtcNow
                };

                _context.OrderProducts.Add(orderProduct);
            }

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order?> UpdateOrderAsync(int orderId, OrderDto updateOrderDto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return null; 
            }

            if (updateOrderDto.UserId.HasValue)
            {
                order.UserId = updateOrderDto.UserId.Value; 
            }

            if (updateOrderDto.OrderShippingMethod != null) order.OrderShippingMethod = updateOrderDto.OrderShippingMethod;
            if (updateOrderDto.CustomerName != null) order.CustomerName = updateOrderDto.CustomerName;
            if (updateOrderDto.CustomerEmail != null) order.CustomerEmail = updateOrderDto.CustomerEmail;
            if (updateOrderDto.CustomerPhone != null) order.CustomerPhone = updateOrderDto.CustomerPhone;
            if (updateOrderDto.ShippingAddress != null) order.ShippingAddress = updateOrderDto.ShippingAddress;
            if (updateOrderDto.BillingAddress != null) order.BillingAddress = updateOrderDto.BillingAddress;
            if (updateOrderDto.CustomerNotes != null) order.CustomerNotes = updateOrderDto.CustomerNotes;
            if (updateOrderDto.TotalAmount.HasValue) order.TotalAmount = updateOrderDto.TotalAmount.Value;
            if (updateOrderDto.VatAmount.HasValue) order.VatAmount = updateOrderDto.VatAmount.Value;
            if (updateOrderDto.DiscountAmount.HasValue) order.DiscountAmount = updateOrderDto.DiscountAmount.Value;
            if (updateOrderDto.PaymentMethod != null) order.PaymentMethod = updateOrderDto.PaymentMethod;
            order.UpdatedAt = DateTime.UtcNow;

            _context.OrderProducts.RemoveRange(order.OrderProducts); // ??? Remove old products ???
            foreach (var orderProductDto in updateOrderDto.OrderProducts)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = orderProductDto.ProductId,
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == orderProductDto.ProductId)?.Name,
                    ProductSku = _context.Products.FirstOrDefault(p => p.Id == orderProductDto.ProductId)?.Sku,
                    Quantity = orderProductDto.Quantity,
                    PricePerUnit = orderProductDto.PricePerUnit,
                    TotalPrice = orderProductDto.TotalPrice,
                    VatRate = orderProductDto.VatRate,
                    VatAmount = orderProductDto.VatAmount,
                    // CreatedAt = DateTime.UtcNow,
                    // UpdatedAt = DateTime.UtcNow
                };

                _context.OrderProducts.Add(orderProduct);
            }

            await _context.SaveChangesAsync();

            return order;
        }
    }
}
