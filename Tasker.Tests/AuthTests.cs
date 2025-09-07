using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tasker.DataAccess.Auth;
using Tasker.Database;

namespace Tasker.Tests;

public class AuthTests
{
    private WebApplicationFactory<API.Program> _webApplicationAPIFactory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void Setup()
    {
        // Fix for multiple EF Core providers registered
        AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);

        _webApplicationAPIFactory = new WebApplicationFactory<API.Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove all DbContextOptions<IdentityContext> registrations
                var dbContextOptionsDescriptors = services
                    .Where(d => d.ServiceType == typeof(DbContextOptions<IdentityContext>))
                    .ToList();
                foreach (var descriptor in dbContextOptionsDescriptors)
                {
                    services.Remove(descriptor);
                }

                // Remove all IdentityContext registrations
                var identityContextDescriptors = services
                    .Where(d => d.ServiceType == typeof(IdentityContext))
                    .ToList();
                foreach (var descriptor in identityContextDescriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<IdentityContext>(options =>   
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the provider and ensure the database is created
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                    identityContext.Database.EnsureCreated();
                }
            });
            
        });
        _client = _webApplicationAPIFactory.CreateClient();

        
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
    public async Task Post_RegisterNewUser_NewUserIsRegistered()
    {
        //Arrange
        RegisterModel model = new RegisterModel();
        model.Email = "test@email.com";
        model.Password = "Password123!";

        //Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", model);

        //Assert
        response.EnsureSuccessStatusCode();

    }

    [TearDown]
    public void TearDown()
    {
        _webApplicationAPIFactory?.Dispose();
        _client?.Dispose();
    }
}
