using BrenkaloWebStoreApi.Data;
using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BrenkaloWebStoreApi.Services
{
    public class ProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public ProductService(WebStoreContext context)
        {
            _context = context;
        }

        private ProductDto MapToProductDto(Product product, int languageId)
        {
            // Find the translation for the specified language
            var productTranslation = product.ProductTranslations.FirstOrDefault(pt => pt.LanguageId == languageId);
            var shortDescription = product.ProductDescriptions
                .FirstOrDefault(pd => pd.LanguageId == languageId && pd.DescriptionTypeId == 1)?.Description;
            var longDescription = product.ProductDescriptions
                .FirstOrDefault(pd => pd.LanguageId == languageId && pd.DescriptionTypeId == 2)?.Description;
            var categoryTranslation = product.Category.CategoryTranslations
                .FirstOrDefault(ct => ct.LanguageId == languageId);

            // Map the data to the ProductDto
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = productTranslation?.Name ?? product.Name!,
                Price = product.Price,
                ShortDescription = shortDescription ?? string.Empty,
                LongDescription = longDescription ?? string.Empty,
                CategoryId = product.CategoryId,
                CategoryName = categoryTranslation?.Name ?? string.Empty,
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

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(int languageId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.CategoryTranslations)
                .Include(p => p.Vat)
                .Include(p => p.ProductReviews)
                .Include(p => p.ProductTranslations)
                .Include(p => p.ProductDescriptions)
                .ThenInclude(pd => pd.Language)
                .ToListAsync();

            return products.Select(product => MapToProductDto(product, languageId));
        }


        public async Task<ProductDto?> GetProductByIdAsync(int id, int languageId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.CategoryTranslations)
                .Include(p => p.ProductReviews)
                .Include(p => p.ProductTranslations)
                .Include(p => p.ProductDescriptions)
                .ThenInclude(pd => pd.Language)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            return MapToProductDto(product, languageId);
        }


        // Retrieve all reviews for a product
        public async Task<IEnumerable<ProductReview>> GetProductReviewsAsync(int productId)
        {
            return await _context.ProductReviews
                .Where(pr => pr.ProductId == productId)
                .Include(pr => pr.User) 
                .ToListAsync();
        }

        // Create a new product review
        public async Task<bool> CreateProductReviewAsync(ProductReviewDto reviewDto)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == reviewDto.ProductId);
            if (product == null)
            {
                return false; 
            }

            // Check if the user exists
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == reviewDto.UserId);
            if (user == null)
            {
                return false; 
            }

            // Create a new review
            var review = new ProductReview
            {
                ProductId = reviewDto.ProductId,
                UserId = reviewDto.UserId,
                Username = reviewDto.Username,
                Stars = reviewDto.Stars,
                ReviewText = reviewDto.ReviewText,
                // CreatedAt = DateTime.UtcNow
            };

            // Add the review to the database
            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

            return true;
        }

        // Update an existing product review
        public async Task<bool> UpdateProductReviewAsync(int reviewId, ProductReviewDto reviewDto)
        {
            var review = await _context.ProductReviews
                .FirstOrDefaultAsync(pr => pr.Id == reviewId);
            if (review == null)
            {
                return false; 
            }

            // Check if the user updating the review is the same as the one who created it
            if (review.UserId != reviewDto.UserId)
            {
                return false; 
            }

            // Update review details
            review.Stars = reviewDto.Stars;
            review.ReviewText = reviewDto.ReviewText;
            // review.CreatedAt = DateTime.UtcNow; 

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
