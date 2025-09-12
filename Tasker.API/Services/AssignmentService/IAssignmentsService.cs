using System;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;

namespace Tasker.API.Services.AssignmentsService;

public interface IAssignmentsService
{
    Task<Result<IEnumerable<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken);
    Task<Result<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken);
    Task<Result<Assignment>> CreateAssignment(long groupId, AssignmentDTO assignment);
    Task<Result<Assignment>> UpdateAssignment(long groupId, long assignmentId, AssignmentDTO updatedAssignment);
    Task<Result<bool>> DeleteAssignment(long groupId, long assignmentId);
    Task<Result<UserAssignment>> AssignTaskToUser(UserAssignmentDTO userAssignment);
    Task<Result<UserAssignment>> UnassignTaskFromUser(UserAssignmentDTO userAssignment);
}
