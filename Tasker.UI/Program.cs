
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Tasker.UI;
using Tasker.UI.Auth;
using Tasker.UI.Services;
using Tasker.UI.Services.AssignmentServiceUI;
using Tasker.UI.Services.UsersManager;

namespace Tasker.UI;

public partial class Program
{
    private static async Task Main(string[] args)
    {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddAuthorizationCore();
        builder.Services.AddSessionStorageServices();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IGroupServiceUI, GroupServiceUI>();
        builder.Services.AddScoped<IAssignmentServiceUI, AssignmentServiceUI>();
        builder.Services.AddScoped<IUserServiceUI, UserServiceUI>();
        builder.Services.AddMudServices();
        builder.Services.AddCascadingAuthenticationState();


        builder.Services.AddScoped((sp) =>
        {
            var configurations = builder.Configuration;
            string apiUrl = configurations["URL:API"]!;
            return new HttpClient
            {
                BaseAddress = new Uri(configurations["URL:API"]!)
            };
        });

        await builder.Build().RunAsync();
    }
}