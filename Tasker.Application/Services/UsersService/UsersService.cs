using Tasker.Domain;

namespace Tasker.Application;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Result<User>> GetUserById(string userId)
    {
        try
        {
            User? user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                return Result.Failure<User>("No user found with the given ID.");
            }
            return Result.Success(user);

        }
        catch (Exception ex)
        {
            return Result.Failure<User>($"Error retrieving user: {ex.Message}");
        }
    }
}
