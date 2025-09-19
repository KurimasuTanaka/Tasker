using Tasker.Domain;

namespace Tasker.Application;

public interface IAssignmentsService
{
    Task<Result<IEnumerable<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken);
    Task<Result<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken);
    Task<Result<Assignment>> CreateAssignment(long groupId, Assignment assignment);
    Task<Result<Assignment>> UpdateAssignment(long groupId, long assignmentId, Assignment updatedAssignment);
    Task<Result<bool>> DeleteAssignment(long groupId, long assignmentId);
    Task<Result<Assignment>> AssignTaskToUser(string userId, long assignmentId);
    Task<Result<Assignment>> UnassignTaskFromUser(string userId, long assignmentId);
}
