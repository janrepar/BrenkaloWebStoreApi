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

        // Map Product to ProductDto
        private ProductDto MapToProductDto(Product product)
        {
            var productDto = new ProductDto
            {
                Name = product.Name!,
                Price = product.Price,
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

            // Get all descriptions in different languages
            var descriptions = product.ProductDescriptions
                .GroupBy(pd => pd.LanguageId)
                .ToList();

            foreach (var group in descriptions)
            {
                var languageId = group.Key;
                foreach (var description in group)
                {
                    if (description.DescriptionTypeId == 1) // Short Description
                    {
                        productDto.ShortDescriptions[languageId] = description.Description;
                    }
                    else if (description.DescriptionTypeId == 2) // Long Description
                    {
                        productDto.LongDescriptions[languageId] = description.Description;
                    }
                }
            }

            // Get category translations for different languages
            var categoryTranslations = product.Category.CategoryTranslations
                .GroupBy(ct => ct.LanguageId)
                .ToList();

            foreach (var group in categoryTranslations)
            {
                var languageId = group.Key;
                foreach (var translation in group)
                {
                    productDto.CategoryNames[languageId] = translation.Name;
                }
            }

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
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

            return products.Select(MapToProductDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
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

            return MapToProductDto(product);
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
