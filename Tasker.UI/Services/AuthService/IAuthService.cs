using System;
using Tasker.DataAccess.Auth;

namespace Tasker.UI.Auth;

public interface IAuthService
{
    Task<string> Register(RegisterModel model);
    Task<string> Login(LoginModel model);
    Task Logout();
}
