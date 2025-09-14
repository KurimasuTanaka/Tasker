using Tasker.Domain;

namespace Tasker.UI.Services;

public interface IUsersManager
{
    public Task<User> GetUserById(string userId);
}
