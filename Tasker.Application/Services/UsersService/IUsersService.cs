using Tasker.Domain;

namespace Tasker.Application;

public interface IUserService
{
    public Task<Result<User>> GetUserById(string userId);
}
