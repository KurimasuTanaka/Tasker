using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;

namespace Tasker.UI.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorageService;
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(ISessionStorageService sessionStorageService, HttpClient httpClient)
    {
        _sessionStorageService = sessionStorageService;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorageService.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwtAuthType");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task NotifyUserAuthentication(string token)
    {
        await _sessionStorageService.SetItemAsync("authToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifyUserLogout()
    {
        await _sessionStorageService.RemoveItemAsync("authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}