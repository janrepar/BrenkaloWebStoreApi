using System;
using System.Collections.Generic;

namespace BrenkaloWebStoreApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public int CategoryId { get; set; }

    public int? SubcategoryId { get; set; }

    public string? Brand { get; set; }

    public string? Manufacturer { get; set; }

    public string? ModelNumber { get; set; }

    public string? MainPictureUrl { get; set; }

    public string? MainProductUrl { get; set; }

    public int? IsFeatured { get; set; }

    public double Price { get; set; }

    public int? VatId { get; set; }

    public int? IsOnSale { get; set; }

    public double? SalePrice { get; set; }

    public string? SaleStartDate { get; set; }

    public string? SaleEndDate { get; set; }

    public double? AverageRating { get; set; }

    public int? NumberOfReviews { get; set; }

    public int? Popularity { get; set; }

    public int? ItemStorage { get; set; }

    public string? StockStatus { get; set; }

    public int? MinimumOrderQuantity { get; set; }

    public int? MaximumOrderQuantity { get; set; }

    public int? IsVisible { get; set; }

    public int? Enabled { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<ProductDescription> ProductDescriptions { get; set; } = new List<ProductDescription>();

    public virtual ICollection<ProductPicture> ProductPictures { get; set; } = new List<ProductPicture>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<ProductTranslation> ProductTranslations { get; set; } = new List<ProductTranslation>();

    public virtual Category? Subcategory { get; set; }

    public virtual Vatrate? Vat { get; set; }
}
