using Tasker.DataAccess;

namespace Tasker.DataAccess.Repositories;

public interface IUserParticipationRepository : IRepository<UserParticipation, long>
{
    public Task<UserParticipation?> GetUserParticipationAsyc(string userId, long groupId);
}
