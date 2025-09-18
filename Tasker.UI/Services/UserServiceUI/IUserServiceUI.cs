using Tasker.Domain;

namespace Tasker.UI.Services;

public interface IUserServiceUI
{
    public Task<User> GetUserById(string userId);
}
