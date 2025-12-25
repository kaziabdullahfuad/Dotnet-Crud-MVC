using BestStoreMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMvc.Repositories
{
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context): base(context)
        {
            
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            return await _context.Products.Where(p=>p.Category==category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await GetAllAsync();
            }

            return await _context.Products.Where(p=>EF.Functions.Like(p.Name,$"%{term}%")).ToListAsync();
        }

         // You can override Generic methods if needed, e.g. include related entities
        public override async Task<Product?> GetByIdAsync(int id)
        {
            // example: if Product had navigations you wanted to include
            return await _context.Products
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
    
}