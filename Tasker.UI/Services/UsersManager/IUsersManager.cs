using System;
using Tasker.DataAccess;

namespace Tasker.UI.Services;

public interface IUsersManager
{
    public Task<User> GetUserById(string userId);
}
