using System;
using System.Net.Http.Json;
using Tasker.Application;
using Tasker.Domain;
using Tasker.Enums;

namespace Tasker.UI.Services;

public class GroupServiceUI : IGroupServiceUI
{
    private readonly HttpClient _httpClient;

    public GroupServiceUI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Group> AddMember(long groupId, string userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/members", userId, cancellationToken);
        response.EnsureSuccessStatusCode();

        var createdGroup = await response.Content.ReadFromJsonAsync<GroupDTO>(cancellationToken: cancellationToken);
        if (createdGroup == null) throw new InvalidOperationException("Failed to add member.");

        return createdGroup.ToDomainObject()!;
    }

    public async Task<Group> CreateGroup(string groupName, CancellationToken cancellationToken = default)
    {
        var group = new GroupDTO { Name = groupName };
        var response = await _httpClient.PostAsJsonAsync("api/groups", group, cancellationToken);
        response.EnsureSuccessStatusCode();

        var createdGroup = await response.Content.ReadFromJsonAsync<GroupDTO>(cancellationToken: cancellationToken);
        if (createdGroup == null) throw new InvalidOperationException("Failed to create group.");

        return createdGroup.ToDomainObject()!;
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

        var groups = await response.Content.ReadFromJsonAsync<List<GroupDTO>>(cancellationToken: cancellationToken);

        if (groups == null) throw new InvalidOperationException("Failed to retrieve groups.");

        return groups.Select(g => g.ToDomainObject()!).ToList();
    }

    public async Task<Group> GetGroupById(long groupId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/groups/{groupId}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var group = await response.Content.ReadFromJsonAsync<GroupDTO>(cancellationToken: cancellationToken);
        if (group == null) throw new InvalidOperationException("Failed to retrieve group.");

        return group.ToDomainObject()!;
    }

    public async Task ChangeUserRole(long groupId, string userId, GroupRole newRole)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/groups/{groupId}/members/{userId}/role", newRole);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Group> UpdateGroup(Group groupToUpdate)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/groups/{groupToUpdate.GroupId}", groupToUpdate.ToDto());
        response.EnsureSuccessStatusCode();

        var group = await response.Content.ReadFromJsonAsync<GroupDTO>();
        if (group is null) throw new InvalidOperationException("Failed to update group.");

        return group.ToDomainObject()!;

    }
}
