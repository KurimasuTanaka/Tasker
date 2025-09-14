using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using Tasker.Application;

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

        try
        {
            return await HandleTokenResponse(response);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}. Registration failed.";
        }
    }

    public async Task<string> Login(LoginModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

        try
        {
            return await HandleTokenResponse(response);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}. Login failed.";
        }
    }

    public async Task Logout()
    {
        await _sessionStorageService.RemoveItemAsync("authToken");
        await (_authStateProvider as CustomAuthStateProvider)!.NotifyUserLogout();
    }

    private async Task<string> HandleTokenResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return error;
        }

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        string? token = result?.Token;

        if (string.IsNullOrWhiteSpace(token)) return "Invalid token";

        await _sessionStorageService.SetItemAsync("authToken", token);
        await (_authStateProvider as CustomAuthStateProvider)!.NotifyUserAuthentication(token);

        return "Authorized successfully";
    }
}

public class TokenResponse { public string Token { get; set; } }


