using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
