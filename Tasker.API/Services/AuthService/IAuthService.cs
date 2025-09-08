using System;
using Tasker.DataAccess.Auth;
using Tasker.DataAccess.DataTransferObjects;

namespace Tasker.API.Services.AuthService;

public interface IAuthService 
{

    Task<Result<string>> Login(LoginModel loginModel);
    Task<Result<string>> Register(RegisterModel registerModel);
}
