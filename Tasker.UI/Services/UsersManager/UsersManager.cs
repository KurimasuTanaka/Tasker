using System;
using System.Net.Http.Json;
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
        return await response.Content.ReadFromJsonAsync<User>();
    }
}
