using System;
using Tasker.DataAccess;

namespace Tasker.UI.Services;

public interface IGroupsManager
{
    Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default);
    Task DeleteGroup(long groupId, CancellationToken cancellationToken = default);
    Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default);
    Task<Group> GetGroupById(long groupId, CancellationToken cancellationToken = default);
}
