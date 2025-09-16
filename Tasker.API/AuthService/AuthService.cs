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

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserRepository userRepository,
        IUserParticipationRepository userParticipationRepository,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _userParticipationRepository = userParticipationRepository;
        _configuration = configuration;
    }

    public async Task<Result<string>> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return Result.Failure<string>("Invalid login");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return Result.Failure<string>("Invalid password");


        List<UserParticipation> userParticipations = (await _userParticipationRepository.GetUserParticipationsAsync(user.Id)).ToList();

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? String.Empty),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

        foreach (var participation in userParticipations)
        {
            claims.Add(new Claim(ClaimTypes.Role, $"{participation.GroupId}:{participation.Role}"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Result.Success(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<Result<string>> Register(RegisterModel registerModel)
    {
        var user = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email, Id = Guid.NewGuid().ToString() };

        try
        {

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _userRepository.AddAsync(new User { UserIdentity = user.Id, FirstName = registerModel.FirstName, LastName = registerModel.LastName });
                return await Login(new LoginModel { Email = registerModel.Email, Password = registerModel.Password });
            }

        }
        catch (Exception ex)
        {
            return Result.Failure<string>($"Registration failed: {ex.Message}");
        }
        return Result.Failure<string>("Registration failed");
    }
}

