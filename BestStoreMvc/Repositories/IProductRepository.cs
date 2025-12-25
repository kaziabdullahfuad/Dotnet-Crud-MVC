using BestStoreMvc.Models;

namespace BestStoreMvc.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<Product>> SearchByNameAsync(string term);

    }
}