using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tasker.Application;
using Tasker.Domain;
namespace Tasker.API;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IUserParticipationRepository _userParticipationRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserRepository userRepository,
        IUserParticipationRepository userParticipationRepository,
        IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _userParticipationRepository = userParticipationRepository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Result<string>> Login(LoginModel model)
    {
        _logger.LogInformation($"User {model.Email} attempting to log in");

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            _logger.LogWarning($"Login failed for {model.Email}: User not found");
            return Result.Failure<string>("Invalid login");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning($"Login failed for {model.Email}: Invalid password");
            return Result.Failure<string>("Invalid password");
        }



        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? String.Empty),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };


        //Get the list of users participations in groups and on their basis form the roles claims where role value is in format "groupId:role"
        //Required for frontend
        List<UserParticipation> userParticipations = (await _userParticipationRepository.GetUserParticipationsAsync(user.Id)).ToList();
        foreach (var participation in userParticipations)
        {
            claims.Add(new Claim(ClaimTypes.Role, $"{participation.GroupId}:{participation.Role}"));
        }

        //Generate JWT token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        _logger.LogInformation($"User {model.Email} logged in successfully");
        return Result.Success(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<Result<string>> Register(RegisterModel registerModel)
    {
        _logger.LogInformation($"Registering new user with email {registerModel.Email}");
        var user = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email, Id = Guid.NewGuid().ToString() };

        try
        {

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _userRepository.AddAsync(new User { UserIdentity = user.Id, FirstName = registerModel.FirstName, LastName = registerModel.LastName });

                // Auto-login after successful registration
                return await Login(new LoginModel { Email = registerModel.Email, Password = registerModel.Password });
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error registering user {registerModel.Email}: {ex.Message}");
            return Result.Failure<string>($"Registration failed: {ex.Message}");
        }

        _logger.LogError($"Registration failed for {registerModel.Email}");
        return Result.Failure<string>("Registration failed");
    }
}

