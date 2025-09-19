using System;
using Tasker.Application;
using Tasker.Domain;
using Tasker.Enums;

namespace Tasker.UI.Services
{
    /// <summary>
    /// Provides UI operations for managing groups, including creation, retrieval, updating, deletion, and member management.
    /// </summary>
    public interface IGroupServiceUI
    {

    /// <summary>
    /// Retrieves all groups 
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of <see cref="Group"/> objects.</returns>
    Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a group by its unique identifier
    /// </summary>
    /// <param name="groupId">The unique identifier of the group to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteGroup(long groupId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing group
    /// </summary>
    /// <param name="group">The updated group entity.</param>
    /// <returns>The updated <see cref="Group"/>.</returns>
    Task<Group> UpdateGroup(Group group);


    /// <summary>
    /// Creates a new group 
    /// </summary>
    /// <param name="groupName">The name of the group to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created <see cref="Group"/>.</returns>
    Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a group by its unique identifier
    /// </summary>
    /// <param name="groupId">The unique identifier of the group to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The requested <see cref="Group"/>.</returns>
    Task<Group> GetGroupById(long groupId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Adds a member to a group 
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The updated <see cref="Group"/> with the new member.</returns>
    Task<Group> AddMember(long groupId, string userId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Changes the role of a user in a group 
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user whose role is to be changed.</param>
    /// <param name="newRole">The new role to assign to the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ChangeUserRole(long groupId, string userId, GroupRole newRole);
    }
}