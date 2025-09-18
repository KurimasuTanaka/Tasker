using Tasker.Domain;
using Tasker.Enums;
namespace Tasker.Application;

public interface IGroupsService
{
    Task<Result<IEnumerable<Group>>> GetAllGroups(string userId, CancellationToken cancellationToken);
    Task<Result<Group>> CreateGroup(Group group, string userId);
    Task<Result<Group>> UpdateGroup(Group group);
    Task<Result<Group>> GetGroupById(long groupId, CancellationToken cancellationToken);
    Task<Result<Group>> AddGroupMember(long groupId, string userId);
    Task<Result<bool>> ChangeUserRole(long groupId, string userId, GroupRole newRole);
    Task<Result<bool>> DeleteGroup(long groupId);
}
