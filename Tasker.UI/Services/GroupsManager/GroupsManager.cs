using System;
using System.Net.Http.Json;
using Tasker.Application;
using Tasker.Domain;

namespace Tasker.UI.Services;

public class GroupsManager : IGroupsManager
{
    private readonly HttpClient _httpClient;

    public GroupsManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddMember(long groupId, string userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/members", userId, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default)
    {
        var group = new Group { Name = groupName };
        var response = await _httpClient.PostAsJsonAsync("api/groups", group, cancellationToken);
        response.EnsureSuccessStatusCode();

        var createdGroup = await response.Content.ReadFromJsonAsync<Group>(cancellationToken: cancellationToken);
        if (createdGroup == null) throw new InvalidOperationException("Failed to create group.");

        return createdGroup;
    }

    public async Task DeleteGroup(long groupId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/groups/{groupId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Group>> GetAllGroups(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("api/groups", cancellationToken);
        response.EnsureSuccessStatusCode();

        var groups = await response.Content.ReadFromJsonAsync<List<Group>>(cancellationToken: cancellationToken);
        return groups ?? new List<Group>();
    }

    public async Task<Group> GetGroupById(long groupId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/groups/{groupId}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var group = await response.Content.ReadFromJsonAsync<Group>(cancellationToken: cancellationToken);
        if (group == null) throw new InvalidOperationException("Failed to retrieve group.");

        return group;
    }

    public async Task<Assignment> CreateAssignment(long groupId, Assignment assignment, CancellationToken cancellationToken = default)
    {
        AssignmentDTO assignmentDto = new AssignmentDTO
        {
            Title = assignment.Title,
            Description = assignment.Description,
            IsCompleted = assignment.IsCompleted,
            GroupId = groupId
        };
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/assignments", assignmentDto);
        response.EnsureSuccessStatusCode();

        var createdAssignment = await response.Content.ReadFromJsonAsync<Assignment>(cancellationToken: cancellationToken);
        if (createdAssignment == null) throw new InvalidOperationException("Failed to create assignment.");

        return createdAssignment;
    }

    public async Task AssignTask(long groupId, long assignmentId, string userId, CancellationToken cancellationToken = default)
    {
        UserAssignmentDTO userAssignmentDto = new UserAssignmentDTO
        {
            UserId = userId,
            AssignmentId = assignmentId
        };
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/userassignments", userAssignmentDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAssignment(long groupId, long assignmentId)
    {
        var response = await _httpClient.DeleteAsync($"api/groups/{groupId}/assignments/{assignmentId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Assignment> UpdateAssignment(long groupId, AssignmentDTO assignmentToUpdate)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/groups/{groupId}/assignments/{assignmentToUpdate.AssignmentId}", assignmentToUpdate);
        response.EnsureSuccessStatusCode();

        var updatedAssignment = await response.Content.ReadFromJsonAsync<Assignment>();
        if (updatedAssignment == null) throw new InvalidOperationException("Failed to update assignment.");

        return updatedAssignment;
    }

    public async Task<Group> UpdateGroup(Group groupToUpdate)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/groups/{groupToUpdate.GroupId}", groupToUpdate.ToDto());
        response.EnsureSuccessStatusCode();

        var group = await response.Content.ReadFromJsonAsync<Group>();
        if(group is null) throw new InvalidOperationException("Failed to update group.");

        return group;

    }
}
