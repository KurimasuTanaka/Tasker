using Tasker.Application;

namespace Tasker.API;

public interface IAuthService
{

    Task<Result<string>> Login(LoginModel loginModel);
    Task<Result<string>> Register(RegisterModel registerModel);
}
