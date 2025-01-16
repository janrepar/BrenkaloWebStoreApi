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

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderShippingMethod = o.OrderShippingMethod,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    CustomerPhone = o.CustomerPhone,
                    ShippingAddress = o.ShippingAddress,
                    ShippingTrackingCode = o.ShippingTrackingCode,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    BillingAddress = o.BillingAddress,
                    CustomerNotes = o.CustomerNotes,
                    OrderStatus = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    VatAmount = o.VatAmount,
                    DiscountAmount = o.DiscountAmount,
                    PaymentMethod = o.PaymentMethod,
                    OrderProducts = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId,
                        ProductName = op.ProductName,
                        ProductSku = op.ProductSku,
                        Quantity = op.Quantity,
                        PricePerUnit = op.PricePerUnit,
                        TotalPrice = op.TotalPrice,
                        VatRate = op.VatRate,
                        VatAmount = op.VatAmount
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderShippingMethod = o.OrderShippingMethod,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    CustomerPhone = o.CustomerPhone,
                    ShippingAddress = o.ShippingAddress,
                    ShippingTrackingCode = o.ShippingTrackingCode,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    BillingAddress = o.BillingAddress,
                    CustomerNotes = o.CustomerNotes,
                    OrderStatus = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    VatAmount = o.VatAmount,
                    DiscountAmount = o.DiscountAmount,
                    PaymentMethod = o.PaymentMethod,
                    OrderProducts = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId,
                        ProductName = op.ProductName,
                        ProductSku = op.ProductSku,
                        Quantity = op.Quantity,
                        PricePerUnit = op.PricePerUnit,
                        TotalPrice = op.TotalPrice,
                        VatRate = op.VatRate,
                        VatAmount = op.VatAmount
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }


        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderProducts)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderShippingMethod = o.OrderShippingMethod,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    CustomerPhone = o.CustomerPhone,
                    ShippingAddress = o.ShippingAddress,
                    ShippingTrackingCode = o.ShippingTrackingCode,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt,
                    BillingAddress = o.BillingAddress,
                    CustomerNotes = o.CustomerNotes,
                    OrderStatus = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    VatAmount = o.VatAmount,
                    DiscountAmount = o.DiscountAmount,
                    PaymentMethod = o.PaymentMethod,
                    OrderProducts = o.OrderProducts.Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId,
                        ProductName = op.ProductName,
                        ProductSku = op.ProductSku,
                        Quantity = op.Quantity,
                        PricePerUnit = op.PricePerUnit,
                        TotalPrice = op.TotalPrice,
                        VatRate = op.VatRate,
                        VatAmount = op.VatAmount
                    }).ToList()
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto createOrderDto, string? token)
        {
            int? userId = null;

            if (!string.IsNullOrWhiteSpace(token))
            {
                // Remove "Bearer " prefix if present and any surrounding quotes
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                token = token.Replace("\"", "").Trim();

                // Fetch the user session using the token
                var userSession = await _context.UserSessions
                    .FirstOrDefaultAsync(us => us.Token == token && us.ValidUntil > DateTime.UtcNow);

                if (userSession == null)
                {
                    throw new UnauthorizedAccessException("Invalid or expired token.");
                }

                // Set the UserId from the user session
                userId = userSession.UserId;
            }

            var order = new Order
            {
                UserId = userId,  
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
                // Get the product from the database
                var product = _context.Products.FirstOrDefault(p => p.Id == orderProductDto.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {orderProductDto.ProductId} not found.");
                }

                // Check if sufficient Item Storage is available
                if (product.ItemStorage < orderProductDto.Quantity)
                {
                    throw new Exception($"Insufficient stock for Product ID {product.Id}. Available stock: {product.ItemStorage}");
                }

                // Update Item Storage and Stock Status
                product.ItemStorage -= orderProductDto.Quantity;
                if (product.ItemStorage == 0)
                {
                    product.StockStatus = "OUT_OF_STOCK";
                }

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

            // Build and return the OrderDto
            var createdOrderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderShippingMethod = order.OrderShippingMethod,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                CustomerNotes = order.CustomerNotes,
                TotalAmount = order.TotalAmount,
                VatAmount = order.VatAmount,
                DiscountAmount = order.DiscountAmount,
                PaymentMethod = order.PaymentMethod,
                OrderStatus = order.OrderStatus,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderProducts = _context.OrderProducts
                    .Where(op => op.OrderId == order.Id)
                    .Select(op => new OrderProductDto
                    {
                        ProductId = op.ProductId,
                        ProductName = op.ProductName,
                        ProductSku = op.ProductSku,
                        Quantity = op.Quantity,
                        PricePerUnit = op.PricePerUnit,
                        TotalPrice = op.TotalPrice,
                        VatRate = op.VatRate,
                        VatAmount = op.VatAmount
                    })
                    .ToList()
            };

            return createdOrderDto;
        }

        // Update order 
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

            // Revert stock for old products
            foreach (var oldProduct in order.OrderProducts)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == oldProduct.ProductId);
                if (product != null)
                {
                    product.ItemStorage += oldProduct.Quantity; 
                    if (product.ItemStorage > 0)
                    {
                        product.StockStatus = "IN_STOCK";
                    }
                }
            }

            _context.OrderProducts.RemoveRange(order.OrderProducts); // ??? Remove old products ???
            foreach (var orderProductDto in updateOrderDto.OrderProducts)
            {
                // Get the product from the database
                var product = _context.Products.FirstOrDefault(p => p.Id == orderProductDto.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {orderProductDto.ProductId} not found.");
                }

                // Check if sufficient Item Storage is available
                if (product.ItemStorage < orderProductDto.Quantity)
                {
                    throw new Exception($"Insufficient stock for Product ID {product.Id}. Available stock: {product.ItemStorage}");
                }

                // Update Item Storage and Stock Status
                product.ItemStorage -= orderProductDto.Quantity;
                if (product.ItemStorage == 0)
                {
                    product.StockStatus = "OUT_OF_STOCK";
                }

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

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return false; 
            }

            order.OrderStatus = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
