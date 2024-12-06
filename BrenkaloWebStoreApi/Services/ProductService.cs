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
    }
}
