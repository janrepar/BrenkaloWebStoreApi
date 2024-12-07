using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IStoreService
    {
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store?> GetStoreByIdAsync(int id);
    }
}
