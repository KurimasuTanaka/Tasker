using System;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;

namespace Tasker.API.Services;

public interface IUserService
{
    public Task<Result<User>> GetUserById(string userId);
}
