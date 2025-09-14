
namespace Tasker.Domain;

public interface IGroupRepository : IRepository<Group, long>
{
    Task<IEnumerable<Group>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
}
