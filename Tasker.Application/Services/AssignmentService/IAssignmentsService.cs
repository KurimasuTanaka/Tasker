using Tasker.Domain;

namespace Tasker.Application;

/// <summary>
/// Provides operations for managing assignments within groups, including creation, retrieval, updating, deletion, and user assignment management.
/// </summary>
public interface IAssignmentsService
{
    /// <summary>
    /// Retrieves all assignments for a specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing a list of assignments for the group.</returns>
    Task<Result<IEnumerable<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a specific assignment by its unique identifier within a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentId">The unique identifier of the assignment.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the requested assignment.</returns>
    Task<Result<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new assignment in a specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignment">The assignment entity to add.</param>
    /// <returns>A result containing the created assignment.</returns>
    Task<Result<Assignment>> CreateAssignment(long groupId, Assignment assignment);

    /// <summary>
    /// Updates an existing assignment in a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentId">The unique identifier of the assignment to update.</param>
    /// <param name="updatedAssignment">The updated assignment entity.</param>
    /// <returns>A result containing the updated assignment.</returns>
    Task<Result<Assignment>> UpdateAssignment(long groupId, long assignmentId, Assignment updatedAssignment);

    /// <summary>
    /// Deletes an assignment from a specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentId">The unique identifier of the assignment to delete.</param>
    /// <returns>A Return type. Return value do not play a role</returns>
    Task<Result<bool>> DeleteAssignment(long groupId, long assignmentId);

    /// <summary>
    /// Assigns a task (assignment) to a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="assignmentId">The unique identifier of the assignment to assign.</param>
    /// <returns>A result containing the assigned assignment.</returns>
    Task<Result<Assignment>> AssignTaskToUser(string userId, long assignmentId);

    /// <summary>
    /// Unassigns a task (assignment) from a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="assignmentId">The unique identifier of the assignment to unassign.</param>
    /// <returns>A result containing the unassigned assignment.</returns>
    Task<Result<Assignment>> UnassignTaskFromUser(string userId, long assignmentId);
}
