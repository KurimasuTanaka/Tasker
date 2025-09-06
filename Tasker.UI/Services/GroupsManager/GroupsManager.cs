using System;
using System.Text.RegularExpressions;

namespace Tasker.UI.Services;

public class GroupsManager : IGroupsManager
{
    public Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroup(Guid groupId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
