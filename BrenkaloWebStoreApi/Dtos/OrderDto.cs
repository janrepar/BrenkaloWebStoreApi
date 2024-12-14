namespace BrenkaloWebStoreApi.Dtos
{
    public class OrderDto
    {
        public int? UserId { get; set; } 
        public string OrderShippingMethod { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? CustomerNotes { get; set; }
        public double? TotalAmount { get; set; }
        public double? VatAmount { get; set; }
        public double? DiscountAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>();
    }

}
