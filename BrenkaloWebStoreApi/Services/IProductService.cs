using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(int languageId);
        Task<ProductDto?> GetProductByIdAsync(int id, int languageId);
        Task<IEnumerable<ProductReview>> GetProductReviewsAsync(int productId);
        Task<bool> CreateProductReviewAsync(ProductReviewDto reviewDto);
        Task<bool> UpdateProductReviewAsync(int reviewId, ProductReviewDto reviewDto);
    }
}
