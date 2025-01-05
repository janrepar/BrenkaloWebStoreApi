using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductReview>> GetProductReviewsAsync(int productId);
        Task<bool> CreateProductReviewAsync(ProductReviewDto reviewDto);
        Task<bool> UpdateProductReviewAsync(int reviewId, ProductReviewDto reviewDto);
    }
}
