using System;
using Tasker.Application;

namespace Tasker.UI.Auth;

public interface IAuthService
{
    Task<string> Register(RegisterModel model);
    Task<string> Login(LoginModel model);
    Task Logout();
}
