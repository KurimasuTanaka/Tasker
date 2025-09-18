using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using NUnit.Framework.Legacy;
using Tasker.Application;
using Tasker.Infrastructure;
using Tasker.UI.Auth;

namespace Tasker.Tests;

public class AuthTests
{
    private WebApplicationFactory<API.Program> _webApplicationAPIFactory = null!;

    private CustomAuthStateProvider _authStateProvider = null!;
    private AuthServiceUI _authService = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void Setup()
    {

        var _sessionStorageServiceMoq = new Mock<ISessionStorageService>();

        _webApplicationAPIFactory = new WebApplicationFactory<API.Program>()
            .WithWebHostBuilder(builder =>
            {
            builder.ConfigureServices(services =>
            {
                var descriptors = services
                 .Where(d =>
                     d.ServiceType == typeof(DbContextOptions<IdentityContext>) ||
                     d.ServiceType == typeof(IDbContextFactory<TaskerContext>) ||
                     d.ServiceType == typeof(IdentityContext) ||
                     d.ServiceType == typeof(TaskerContext)
                 )
                 .ToList();

                foreach (var d in descriptors)
                    services.Remove(d);

                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlite("Data Source=./IdentityTestDb"));

                services.AddDbContextFactory<TaskerContext>(options =>
                    options.UseSqlite("Data Source=./TaskerTestDb"));


                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<TaskerContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", "ThisIsASecretKeyForJwtTokenDontShare" },
                    { "Jwt:Issuer", "TaskerAPI" },
                    { "Jwt:Audience", "TaskerAPIClient" },
                });
            });
    });

        _client = _webApplicationAPIFactory.CreateClient();

        _authStateProvider = new(_sessionStorageServiceMoq.Object, _client);
        _authService = new AuthServiceUI(_client, _authStateProvider, _sessionStorageServiceMoq.Object);
    }

    [Test]
    public async Task Get_SentRequestTSwaggerPage_SuccessfulStatusCodeReturned()
    {
        //Act
        var response = await _client.GetAsync("/swagger/index.html");
        //Assert
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task Post_RegisterNewUser_NewUserIsRegisteredAndAuthorized()
    {
        //Arrange
        RegisterModel model = new RegisterModel();
        model.Email = "test@email.com";
        model.Password = "Password123!";
        model.FirstName = "Test";
        model.LastName = "Testsson";

        //Act
        string returnString = await _authService.Register(model);

        //Assert
        Assert.That(returnString, Is.EqualTo("Authorized successfully"));
    }

    [Test]
    public async Task Post_LoginToNewAccount_UserIsAuthorized()
    {
        //Arrange
        var registerModel = new RegisterModel
        {
            Email = "test@email.com",
            Password = "Password123!",
            FirstName = "Test",
            LastName = "Testsson"
        };

        await _authService.Register(registerModel);

        //Act

        var loginModel = new LoginModel
        {
            Email = "test@email.com",
            Password = "Password123!"

        };

        string result = await _authService.Login(loginModel);

        //Assert
        Assert.That(result, Is.EqualTo("Authorized successfully"));

    }


    [TearDown]
    public void TearDown()
    {
        _webApplicationAPIFactory?.Dispose();
        _client?.Dispose();
    }
}
