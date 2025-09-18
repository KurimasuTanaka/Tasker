using System;
using Tasker.Domain;

namespace Tasker.UI.Services.AssignmentServiceUI;

public interface IAssignmentServiceUI
{
    Task<Assignment> CreateAssignment(long groupId, Assignment assignment, CancellationToken cancellationToken = default);
    Task<Assignment> AssignTask(long groupId, long assignmentId, string userId);
    Task UnassignTask(long groupId, long assignmentId, string userId);
    Task DeleteAssignment(long groupId, long assignmentId);
    Task<Assignment> UpdateAssignment(long groupId, Assignment assignmentToUpdate);
}
