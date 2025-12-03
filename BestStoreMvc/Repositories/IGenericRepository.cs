namespace BestStoreMvc.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        // optionally expose save if you prefer unit-of-work pattern
        Task SaveChangesAsync();
    }
}