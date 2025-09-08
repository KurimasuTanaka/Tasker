using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Tasker.DataAccess;
using Tasker.DataAccess.Auth;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;

namespace Tasker.API.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserRepository _userRepository;
    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Login(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return Result.Failure<string>("Invalid login");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return Result.Failure<string>("Invalid password");


        var dbUser = await _userRepository.GetAsync(user.Id);
        if (dbUser == null) return Result.Failure<string>("User not found in database");

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, "User") // Default role
            };

        foreach (var participation in dbUser.Participations)
        {
            claims.Add(new Claim(ClaimTypes.Role, $"{participation.GroupId}:{participation.Role}"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyAtLeast32CharsLong"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourapp.com",
            audience: "yourapp.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), 
            signingCredentials: creds);

        return Result.Success(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<Result<string>> Register(RegisterModel registerModel)
    {
        var user = new IdentityUser { UserName = registerModel.Email, Email = registerModel.Email, Id = Guid.NewGuid().ToString() };
        var result = await _userManager.CreateAsync(user, registerModel.Password);

        if (result.Succeeded)
        {
            await _userRepository.AddAsync(new User { UserIdentity = user.Id });
            return await Login(new LoginModel { Email = registerModel.Email, Password = registerModel.Password });
        }

        return Result.Failure<string>("Registration failed");
    }
}

