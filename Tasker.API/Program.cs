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

        

        builder.Services.AddCors(options =>
        {
            var configurations = builder.Configuration;

            options.AddPolicy("AllowFrontend",
                policy => policy.WithOrigins(configurations["URL:Frontend"]!)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials());
        });

        builder.Services.AddSignalR();

        builder.Services.AddDbContextFactory<TaskerContext>(options =>
        {
            var configurations = builder.Configuration;
            var connectionString = configurations["ConnectionStrings:TaskerConnection"];
            options.UseSqlite(connectionString);
        });

        builder.Services.AddDbContext<IdentityContext>(options =>
        {
            var configurations = builder.Configuration;
            var connectionString = configurations["ConnectionStrings:IdentityConnection"];
            options.UseSqlite(connectionString);
        });

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
            var configurations = builder.Configuration;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, 
                ValidateAudience = true,
                ValidateLifetime = true, 
                ValidateIssuerSigningKey = true,
                ValidIssuer = configurations["Jwt:Issuer"],
                ValidAudience = configurations["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations["Jwt:Key"]!))
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
        app.MapHub<NotificationHub>("/notificationsHub");

        app.Run();
    }
}