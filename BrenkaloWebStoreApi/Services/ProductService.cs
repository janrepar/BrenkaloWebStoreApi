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

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Include(p => p.Vat)

                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Include(p => p.Vat)
                .FirstOrDefaultAsync(p => p.Id == id);
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
