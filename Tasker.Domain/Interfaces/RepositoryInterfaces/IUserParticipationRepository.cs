
namespace Tasker.Domain;

public interface IUserParticipationRepository : IRepository<UserParticipation, (string userId, long groupId)>
{
    public Task<IEnumerable<UserParticipation>> GetUserParticipationsAsync(string userId, CancellationToken cancellationToken = default);
}
