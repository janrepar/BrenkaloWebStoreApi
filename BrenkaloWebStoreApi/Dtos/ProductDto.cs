namespace BrenkaloWebStoreApi.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string ShortDescription { get; set; } = null!;
        public string LongDescription { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        // public string SubcategoryName { get; set; } = null!;
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
