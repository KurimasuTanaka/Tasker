using Tasker.DataAccess;

namespace Tasker.DataAccess.Repositories;

public interface IGroupRepository : IRepository<Group, long>
{
    Task<IEnumerable<Group>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
}
