using Tasker.Domain;
using Tasker.Enums;
namespace Tasker.Application;

/// <summary>
/// Provides group management operations such as creating, updating, retrieving, and deleting groups,
/// as well as managing group members and their roles.
/// </summary>
public interface IGroupsService
{
    /// <summary>
    /// Retrieves all groups associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose groups are to be retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing a collection of groups.</returns>
    Task<Result<IEnumerable<Group>>> GetAllGroups(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new group and associates it with a user.
    /// </summary>
    /// <param name="group">The group entity to create.</param>
    /// <param name="userId">The ID of the user creating the group.</param>
    /// <returns>A result containing the created group.</returns>
    Task<Result<Group>> CreateGroup(Group group, string userId);

    /// <summary>
    /// Updates the details of an existing group.
    /// </summary>
    /// <param name="group">The group entity with updated information.</param>
    /// <returns>A result containing the updated group.</returns>
    Task<Result<Group>> UpdateGroup(Group group);

    /// <summary>
    /// Retrieves a group by its unique identifier.
    /// </summary>
    /// <param name="groupId">The ID of the group to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result containing the requested group.</returns>
    Task<Result<Group>> GetGroupById(long groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a user as a member to a group.
    /// </summary>
    /// <param name="groupId">The ID of the group.</param>
    /// <param name="userId">The ID of the user to add.</param>
    /// <returns>A result containing the updated group.</returns>
    Task<Result<Group>> AddGroupMember(long groupId, string userId);

    /// <summary>
    /// Changes the role of a user within a group.
    /// </summary>
    /// <param name="groupId">The ID of the group.</param>
    /// <param name="userId">The ID of the user whose role is to be changed.</param>
    /// <param name="newRole">The new role to assign to the user.</param>
    /// <returns>A Return type. Return value do not play a role.</returns>
    Task<Result<bool>> ChangeUserRole(long groupId, string userId, GroupRole newRole);

    /// <summary>
    /// Deletes a group by its unique identifier.
    /// </summary>
    /// <param name="groupId">The ID of the group to delete.</param>
    /// <returns>A Return type. Return value do not play a role</returns>
    Task<Result<bool>> DeleteGroup(long groupId);
}
