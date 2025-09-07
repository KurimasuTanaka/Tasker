using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Tasker.DataAccess.Auth;

namespace Tasker.UI.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ISessionStorageService _sessionStorageService;


    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ISessionStorageService sessionStorageService)
    {
        _httpClient = http;
        _authStateProvider = authStateProvider;
        _sessionStorageService = sessionStorageService;
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
        await _sessionStorageService.SetItemAsync("authToken", token);
        await (_authStateProvider as CustomAuthStateProvider)!.NotifyUserAuthentication(token);

        return true;
    }

    public async Task Logout()
    {
        await _sessionStorageService.RemoveItemAsync("authToken");
        await (_authStateProvider as CustomAuthStateProvider)!.NotifyUserLogout();
    }
}

public class TokenResponse { public string Token { get; set; } }


