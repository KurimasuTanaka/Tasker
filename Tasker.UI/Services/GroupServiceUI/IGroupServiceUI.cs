using System;
using Tasker.Application;
using Tasker.Domain;

namespace Tasker.UI.Services;

public interface IGroupServiceUI
{
    Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default);
    Task DeleteGroup(long groupId, CancellationToken cancellationToken = default);
    Task<Group> UpdateGroup(Group group);
    Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default);
    Task<Group> GetGroupById(long groupId, CancellationToken cancellationToken = default);
    Task<Group> AddMember(long groupId, string userId, CancellationToken cancellationToken = default);

}
