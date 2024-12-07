using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
    }
}
