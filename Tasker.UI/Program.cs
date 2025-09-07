
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tasker.UI;
using Tasker.UI.Auth;
using Tasker.UI.Services;

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
        builder.Services.AddScoped<IGroupsManager, GroupsManager>();

        builder.Services.AddCascadingAuthenticationState();


        builder.Services.AddScoped(sp =>
            new HttpClient { BaseAddress = new Uri("https://localhost:5000/") });
        await builder.Build().RunAsync();
    }
}