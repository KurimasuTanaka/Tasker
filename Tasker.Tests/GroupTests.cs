using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using NUnit.Framework.Legacy;
using Tasker.DataAccess.Auth;
using Tasker.Database;
using Tasker.UI.Auth;
using Tasker.UI.Services;

namespace Tasker.Tests;

public class GroupTests
{
    private WebApplicationFactory<API.Program> _webApplicationAPIFactory = null!;

    private CustomAuthStateProvider _authStateProvider = null!;
    private AuthService _authService = null!;
    private GroupsManager _groupManager = null!;
    private HttpClient _client = null!;

    private UserModel _userGroupCreator = null!;
    private string _userGroupCreatorPassword = "Password123!";
    private string _userGroupCreatorEmail = "testuser@example.com";


    private UserModel _userGroupMember = null!;
    private string _userGroupMemberPassword = "Password123!";
    private string _userGroupMemberEmail = "testusermember@example.com";

    [SetUp]
    public void Setup()
    {

        var _sessionStorageServiceMoq = new Mock<ISessionStorageService>();

        _webApplicationAPIFactory = new WebApplicationFactory<API.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registrations
                    var descriptors = services
                        .Where(d =>
                            d.ServiceType == typeof(DbContextOptions<IdentityContext>) ||
                            d.ServiceType == typeof(DbContextOptions<TaskerContext>) ||
                            d.ServiceType == typeof(IdentityContext) ||
                            d.ServiceType == typeof(TaskerContext)
                            )
                        .ToList();

                    foreach (var d in descriptors)
                    {
                        services.Remove(d);
                    }

                    // Register in-memory databases instead
                    services.AddDbContext<IdentityContext>(options =>
                        options.UseSqlite("Data Source=./IdentityTestDb"));

                    services.AddDbContext<TaskerContext>(options =>
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
            });

        _client = _webApplicationAPIFactory.CreateClient();

        _authStateProvider = new(_sessionStorageServiceMoq.Object, _client);
        _authService = new AuthService(_client, _authStateProvider, _sessionStorageServiceMoq.Object);
        _groupManager = new GroupsManager(_client);


    }

    [Test]
    public async Task Post_SentRequestToCreateGroup_NewGroupIsCreated()
    {
        //Arrange
        string testGroupName = "Test Group";

        await _authService.Register(new RegisterModel
        {
            Email = _userGroupCreatorEmail,
            Password = _userGroupCreatorPassword
        });

        await _authService.Register(new RegisterModel
        {
            Email = _userGroupMemberEmail,
            Password = _userGroupMemberPassword
        });

        var loginKey = await _authService.Login(new LoginModel
        {
            Email = _userGroupCreatorEmail,
            Password = _userGroupCreatorPassword
        });

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginKey);
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //Act
        var group = await _groupManager.CreateGroup(testGroupName);
        //Assert

        Assert.That(group, Is.Not.Null);
        Assert.That(group.Name, Is.EqualTo(testGroupName));

    }





    [TearDown]
    public void TearDown()
    {
        _webApplicationAPIFactory?.Dispose();
        _client?.Dispose();
    }
}
