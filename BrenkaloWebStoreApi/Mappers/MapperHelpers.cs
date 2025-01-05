namespace BrenkaloWebStoreApi.Mappers
{
    public class MapperHelpers
    {
		// Map Product to ProductDto
		private ProductDto MapToProductDto(Product product)
		{
			var productDto = new ProductDto
			{
				Name = product.Name!,
				Price = product.Price,
				CategoryName = product.Category.Name,
				Brand = product.Brand,
				Manufacturer = product.Manufacturer,
				ModelNumber = product.ModelNumber,
				MainPictureUrl = product.MainPictureUrl,
				MainProductUrl = product.MainProductUrl,
				IsFeatured = product.IsFeatured,
				IsOnSale = product.IsOnSale,
				SalePrice = product.SalePrice,
				SaleStartDate = product.SaleStartDate,
				SaleEndDate = product.SaleEndDate,
				AverageRating = product.AverageRating ?? 0,
				NumberOfReviews = product.NumberOfReviews ?? 0,
				ItemStorage = product.ItemStorage,
				StockStatus = product.StockStatus,
				Reviews = product.ProductReviews.Select(pr => new ProductReviewDto
				{
					Username = pr.Username,
					Stars = pr.Stars,
					ReviewText = pr.ReviewText,
				}).ToList()
			};
			var defaultLanguage = 2;
			var descriptions = product.ProductDescriptions
				.Where(pd => pd.LanguageId == defaultLanguage)
				.ToList();

			var shortDescription = descriptions.FirstOrDefault(pd => pd.DescriptionTypeId == 1)?.Description;
			var longDescription = descriptions.FirstOrDefault(pd => pd.DescriptionTypeId == 2)?.Description;

			productDto.ShortDescription = shortDescription ?? string.Empty;
			productDto.LongDescription = longDescription ?? string.Empty;

			return productDto;
		}
	}
}
