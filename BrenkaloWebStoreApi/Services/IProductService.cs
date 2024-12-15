using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductReview>> GetProductReviewsAsync(int productId);
        Task<bool> CreateProductReviewAsync(ProductReviewDto reviewDto);
        Task<bool> UpdateProductReviewAsync(int reviewId, ProductReviewDto reviewDto);
    }
}
