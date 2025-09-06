using System;
using System.Text.RegularExpressions;

namespace Tasker.UI.Services;

public interface IGroupsManager
{
    Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default);
    Task DeleteGroup(Guid groupId, CancellationToken cancellationToken = default);
    Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default);
}
