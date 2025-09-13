using System;
using System.Security.Claims;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;

namespace Tasker.API.Services.GroupsService;

public interface IGroupsService
{
    Task<Result<IEnumerable<Group>>> GetAllGroups(string userId, CancellationToken cancellationToken);
    Task<Result<Group>> CreateGroup(Group group, string userId);
    Task<Result<Group>> UpdateGroup(Group group);
    Task<Result<Group>> GetGroupById(long groupId, CancellationToken cancellationToken);
    Task<Result<Group>> AddGroupMember(long groupId, string userId);
    Task<Result<bool>> DeleteGroup(long groupId);
}
