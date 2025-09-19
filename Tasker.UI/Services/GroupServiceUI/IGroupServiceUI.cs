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
    /// <returns>A list of <see cref="Group"/> objects.</returns>
    Task<List<Group>> GetAllGroups();


    /// <summary>
    /// Deletes a group by its unique identifier
    /// </summary>
    /// <param name="groupId">The unique identifier of the group to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteGroup(long groupId);


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
    /// <returns>The created <see cref="Group"/>.</returns>
    Task<Group> CreateGroup(string groupName);


    /// <summary>
    /// Retrieves a group by its unique identifier
    /// </summary>
    /// <param name="groupId">The unique identifier of the group to retrieve.</param>
    /// <returns>The requested <see cref="Group"/>.</returns>
    Task<Group> GetGroupById(long groupId);


    /// <summary>
    /// Adds a member to a group 
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <returns>The updated <see cref="Group"/> with the new member.</returns>
    Task<Group> AddMember(long groupId, string userId);


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