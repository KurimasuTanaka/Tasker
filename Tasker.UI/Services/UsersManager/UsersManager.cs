using System;
using System.Net.Http.Json;
using Tasker.Application;
using Tasker.Domain;

namespace Tasker.UI.Services.UsersManager;

public class UsersManager : IUsersManager
{
    private readonly HttpClient _httpClient;

    public UsersManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> GetUserById(string userId)
    {
        var response = await _httpClient.GetAsync($"api/users/{userId}");
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserDTO>();
        if (user == null) throw new InvalidOperationException("Failed to retrieve user.");

        return user.ToDomainObject()!;
    }
}
