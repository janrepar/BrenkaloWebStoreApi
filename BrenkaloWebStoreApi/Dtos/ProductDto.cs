namespace BrenkaloWebStoreApi.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        // Updated to hold descriptions for multiple languages
        public Dictionary<int, string> ShortDescriptions { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> LongDescriptions { get; set; } = new Dictionary<int, string>();

        // Updated to hold category names for multiple languages
        public Dictionary<int, string> CategoryNames { get; set; } = new Dictionary<int, string>();

        public string? Brand { get; set; }
        public string? Manufacturer { get; set; }
        public string? ModelNumber { get; set; }
        public string? MainPictureUrl { get; set; }
        public string? MainProductUrl { get; set; }
        public int? IsFeatured { get; set; }
        public int? IsOnSale { get; set; }
        public double? SalePrice { get; set; }
        public string? SaleStartDate { get; set; }
        public string? SaleEndDate { get; set; }

        // Product review summary
        public double AverageRating { get; set; }
        public int NumberOfReviews { get; set; }

        // Stock status
        public int? ItemStorage { get; set; }
        public string? StockStatus { get; set; }

        // Product Reviews
        public List<ProductReviewDto> Reviews { get; set; } = new List<ProductReviewDto>();
    }
}
