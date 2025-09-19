using Tasker.Domain;

namespace Tasker.UI.Services
{
    /// <summary>
    /// Provides UI operations for retrieving user information.
    /// </summary>
    public interface IUserServiceUI
    {
    /// <summary>
    /// Retrieves a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The requested User.</returns>
    public Task<User> GetUserById(string userId);
    }
}

