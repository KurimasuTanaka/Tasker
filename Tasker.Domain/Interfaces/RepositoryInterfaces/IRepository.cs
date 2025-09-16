namespace Tasker.Domain;

public interface IRepository<T, TKey> where T : class
{
    // Create operations
    Task<T?> AddAsync(T entity);
    
    // Read operations
    Task<T?> GetAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    
    // Update operations
    Task<T?> UpdateAsync(T entity);
    
    // Delete operations
    Task<bool> DeleteAsync(TKey id);
}
