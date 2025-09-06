namespace Tasker.DataAccess.Repositories;

public interface IRepository<T> where T : class
{
    // Create operations
    Task<T> AddAsync(T entity);
    
    // Read operations
    Task<T?> GetAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    
    // Update operations
    Task<T> UpdateAsync(T entity);
    
    // Delete operations
    Task<bool> DeleteAsync(object id);
}
