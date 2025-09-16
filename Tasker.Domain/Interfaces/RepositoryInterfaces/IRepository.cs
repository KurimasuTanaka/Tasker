namespace Tasker.Domain;

public interface IRepository<T, TKey> where T : class
{
    Task<T?> AddAsync(T entity);
    Task<T?> GetAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> UpdateAsync(T entity);
    Task<bool> DeleteAsync(TKey id);
}
