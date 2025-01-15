namespace BrenkaloWebStoreApi.Dtos
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSku { get; set; }
        public double PricePerUnit { get; set; }
        public double TotalPrice { get; set; }
        public double? VatRate { get; set; }
        public double? VatAmount { get; set; }
    }
}
