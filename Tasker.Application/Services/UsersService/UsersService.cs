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
        User? user = await _userRepository.GetAsync(userId);
        if(user == null)
        {
            return Result.Failure<User>("No user found with the given ID.");
        }
        return Result.Success(user);
    }
}
