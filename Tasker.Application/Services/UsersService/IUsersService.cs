using Tasker.Domain;

namespace Tasker.Application;

/// <summary>
/// Provides user-related operations such as retrieving user information by user ID.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A result containing the user information.</returns>
    Task<Result<User>> GetUserById(string userId);
}
