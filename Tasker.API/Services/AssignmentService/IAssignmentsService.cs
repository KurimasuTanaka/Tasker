using System;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;

namespace Tasker.API.Services.AssignmentsService;

public interface IAssignmentsService
{
    Task<Result<IEnumerable<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken);
    Task<Result<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken);
    Task<Result<Assignment>> CreateAssignment(long groupId, Assignment assignment);
    Task<Result<Assignment>> UpdateAssignment(long groupId, long assignmentId, Assignment updatedAssignment);
    Task<Result<bool>> DeleteAssignment(long groupId, long assignmentId);
    Task<Result<Assignment>> AssignTaskToUser(long groupId, long assignmentId, long userId);
    Task<Result<Assignment>> UnassignTaskFromUser(long groupId, long assignmentId, long userId);
}
