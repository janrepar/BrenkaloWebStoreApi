namespace BrenkaloWebStoreApi.Dtos
{
    public class UserAddressDto
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int? IsDefault { get; set; }
    }
}
