
namespace Tasker.Domain;

public interface IUserParticipationRepository : IRepository<UserParticipation, long>
{
    public Task<UserParticipation?> GetUserParticipationAsyc(string userId, long groupId);
}
