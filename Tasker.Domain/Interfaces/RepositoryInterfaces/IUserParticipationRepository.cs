
namespace Tasker.Domain;

public interface IUserParticipationRepository : IRepository<UserParticipation, (string userId, long groupId)>
{
    public Task<IEnumerable<UserParticipation>> GetUserParticipationsAsyc(string userId, CancellationToken cancellationToken = default);
}
