using System;
using Tasker.Domain;

namespace Tasker.UI.Services.AssignmentServiceUI
{
    /// <summary>
    /// Provides UI operations for managing assignments within groups, including creation, retrieval, updating, deletion, and user assignment management.
    /// </summary>
    public interface IAssignmentServiceUI
    {

    /// <summary>
    /// Creates a new assignment in a specific group asynchronously.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignment">The assignment entity to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created <see cref="Assignment"/>.</returns>
    Task<Assignment> CreateAssignment(long groupId, Assignment assignment, CancellationToken cancellationToken = default);


    /// <summary>
    /// Assigns a task (assignment) to a user asynchronously.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentId">The unique identifier of the assignment.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The assigned <see cref="Assignment"/>.</returns>
    Task<Assignment> AssignTask(long groupId, long assignmentId, string userId);


    /// <summary>
    /// Unassigns a task (assignment) from a user asynchronously.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentId">The unique identifier of the assignment.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The unassigned <see cref="Assignment"/>.</returns>
    Task<Assignment> UnassignTask(long groupId, long assignmentId, string userId);


    /// <summary>
    /// Deletes an assignment from a group asynchronously.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentId">The unique identifier of the assignment to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAssignment(long groupId, long assignmentId);

    /// <summary>
    /// Updates an existing assignment in a group asynchronously.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="assignmentToUpdate">The updated assignment entity.</param>
    /// <returns>The updated <see cref="Assignment"/>.</returns>
    Task<Assignment> UpdateAssignment(long groupId, Assignment assignmentToUpdate);
    }
}
