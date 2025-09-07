using System;
using System.Net.Http.Json;
using Tasker.DataAccess;

namespace Tasker.UI.Services;

public class GroupsManager : IGroupsManager
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GroupsManager> _logger;

    public GroupsManager(HttpClient httpClient, ILogger<GroupsManager> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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

        return await response.Content.ReadFromJsonAsync<Group>(cancellationToken: cancellationToken);
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

        return await response.Content.ReadFromJsonAsync<Group>(cancellationToken: cancellationToken);
    }
}
