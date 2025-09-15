using System;
using Tasker.Application;
using Tasker.Domain;

namespace Tasker.UI.Services;

public interface IGroupsManager
{
    Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default);
    Task DeleteGroup(long groupId, CancellationToken cancellationToken = default);
    Task<Group> UpdateGroup(Group group);
    Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default);
    Task<Group> GetGroupById(long groupId, CancellationToken cancellationToken = default);
    Task AddMember(long groupId, string userId, CancellationToken cancellationToken = default);
    Task<Assignment> CreateAssignment(long groupId, Assignment assignment, CancellationToken cancellationToken = default);
    Task<Assignment> AssignTask(long groupId, long assignmentId, string userId, CancellationToken cancellationToken = default);
    Task<Assignment> DeleteAssignment(long groupId, long assignmentId);
    Task<Assignment> UpdateAssignment(long groupId, AssignmentDTO assignmentToUpdate);
}
