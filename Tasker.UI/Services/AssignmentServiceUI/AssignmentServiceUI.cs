using System;
using System.Net.Http.Json;
using Tasker.Application;
using Tasker.Domain;

namespace Tasker.UI.Services.AssignmentServiceUI;

public class AssignmentServiceUI : IAssignmentServiceUI
{
    private readonly HttpClient _httpClient;

    public AssignmentServiceUI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Assignment> CreateAssignment(long groupId, Assignment assignment, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/assignments", assignment.ToDto());
        response.EnsureSuccessStatusCode();

        var createdAssignment = await response.Content.ReadFromJsonAsync<AssignmentDTO>(cancellationToken: cancellationToken);
        if (createdAssignment == null) throw new InvalidOperationException("Failed to create assignment.");

        return createdAssignment.ToDomainObject()!;
    }

    public async Task<Assignment> AssignTask(long groupId, long assignmentId, string userId)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/groups/{groupId}/user/{userId}/assignments/{assignmentId}", new { });
        response.EnsureSuccessStatusCode();

        var createdAssignment = await response.Content.ReadFromJsonAsync<AssignmentDTO>();
        if (createdAssignment == null) throw new InvalidOperationException("Failed to update assignment.");

        return createdAssignment.ToDomainObject()!;

    }

    public async Task<Assignment> UnassignTask(long groupId, long assignmentId, string userId)
    {
        var response = await _httpClient.DeleteAsync($"api/groups/{groupId}/user/{userId}/assignments/{assignmentId}");
        response.EnsureSuccessStatusCode();

        var createdAssignment = await response.Content.ReadFromJsonAsync<AssignmentDTO>();
        if (createdAssignment == null) throw new InvalidOperationException("Failed to update assignment.");

        return createdAssignment.ToDomainObject()!;

    }


    public async Task DeleteAssignment(long groupId, long assignmentId)
    {
        var response = await _httpClient.DeleteAsync($"api/groups/{groupId}/assignments/{assignmentId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Assignment> UpdateAssignment(long groupId, Assignment assignmentToUpdate)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/groups/{groupId}/assignments/{assignmentToUpdate.AssignmentId}", assignmentToUpdate.ToDto());
        response.EnsureSuccessStatusCode();

        var updatedAssignment = await response.Content.ReadFromJsonAsync<AssignmentDTO>();
        if (updatedAssignment == null) throw new InvalidOperationException("Failed to update assignment.");

        return updatedAssignment.ToDomainObject()!;
    }
}
