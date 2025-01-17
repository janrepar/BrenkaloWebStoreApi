﻿namespace BrenkaloWebStoreApi.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string ShortDescription { get; set; } 
        public string LongDescription { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
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
