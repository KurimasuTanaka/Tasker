using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Tasker.DataAccess.Auth;

namespace Tasker.UI.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IMemoryCache _memoryCache;

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, IMemoryCache memoryCache)
    {
        _httpClient = http;
        _authStateProvider = authStateProvider;
        _memoryCache = memoryCache;
    }

    public async Task<string> Register(RegisterModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> Login(LoginModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        var token = result?.Token;
        if (string.IsNullOrWhiteSpace(token)) return false;
        _memoryCache.Set("authToken", token);
        await (_authStateProvider as CustomAuthStateProvider)!.NotifyUserAuthentication(token);

        return true;
    }

    public async Task Logout()
    {
        _memoryCache.Remove("authToken");
        await (_authStateProvider as CustomAuthStateProvider)!.NotifyUserLogout();
    }
}

public class TokenResponse { public string Token { get; set; } }


