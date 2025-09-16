using Microsoft.Extensions.Logging;
using Tasker.Domain;

namespace Tasker.Application;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    public async Task<Result<User>> GetUserById(string userId)
    {
        _logger.LogInformation($"Retrieving user {userId}");

        try
        {
            User? user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"No user found with ID {userId}");
                return Result.Failure<User>("No user found with the given ID.");
            }

            _logger.LogInformation($"Successfully retrieved user {userId}");
            return Result.Success(user);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user {userId}: {ex.Message}");
            return Result.Failure<User>($"Error retrieving user: {ex.Message}");
        }
    }
}
