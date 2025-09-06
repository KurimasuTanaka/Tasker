using System;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Tasker.UI.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<string> Register(RegisterModel model)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", model);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> Login(LoginModel model)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", model);
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        var token = result?.Token;
        if (string.IsNullOrWhiteSpace(token)) return false;
        await _localStorage.SetItemAsync("authToken", token);
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);

        return true;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}

public class RegisterModel { public string Email { get; set; } public string Password { get; set; } }
public class LoginModel { public string Email { get; set; } public string Password { get; set; } }
public class TokenResponse { public string Token { get; set; } }


