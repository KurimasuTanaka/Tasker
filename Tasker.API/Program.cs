using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tasker.Application;
using Tasker.Domain;
using Tasker.Infrastructure;
namespace Tasker.API;


public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy => policy.WithOrigins("https://localhost:7001")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials());
        });

        builder.Services.AddDbContextFactory<TaskerContext>(options =>
            options.UseSqlite("Data Source=./tasker.db"));

        builder.Services.AddDbContext<IdentityContext>(options =>
            options.UseSqlite("Data Source=./identity.db"));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Проверять issuer
                ValidateAudience = true, // Проверять audience
                ValidateLifetime = true, // Проверять срок
                ValidateIssuerSigningKey = true, // Проверять подпись
                ValidIssuer = "yourapp.com", // Твой issuer (можно из конфига)
                ValidAudience = "yourapp.com", // Твой audience
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyAtLeast32CharsLong")) // Секретный ключ, храни в secrets!
            };
        });


        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserAssignmentRepository, UserAssignmentRepository>();
        builder.Services.AddScoped<IUserParticipationRepository, UserParticipationRepository>();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IGroupsService, GroupsService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAssignmentsService, AssignmentsService>();

        builder.Services.AddSwaggerGen();

        builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

                


        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwaggerUI();
        app.UseSwagger();
        app.MapControllers();
        app.Run();
    }
}