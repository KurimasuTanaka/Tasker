using System;
using Tasker.Application;

namespace Tasker.UI.Auth
{
    /// <summary>
    /// Provides UI operations for user authentication, including login, logout, and registration.
    /// </summary>
    public interface IAuthServiceUI
    {
    /// <summary>
    /// Registers a new user asynchronously with the provided credentials.
    /// </summary>
    /// <param name="model">The registration data model containing user credentials.</param>
    /// <returns>A string representing the registration result or token.</returns>
    Task<string> Register(RegisterModel model);

    /// <summary>
    /// Authenticates a user asynchronously with the provided credentials.
    /// </summary>
    /// <param name="model">The login data model containing user credentials.</param>
    /// <returns>A string representing the authentication result or token.</returns>
    Task<string> Login(LoginModel model);

    /// <summary>
    /// Logs out the current user 
    /// </summary>
    Task Logout();
    }
}

