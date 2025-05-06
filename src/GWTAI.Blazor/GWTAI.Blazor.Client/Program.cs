using GWTAI.Blazor.Client;
using GWTAI.Blazor.Client.Services;
using GWTAI.Blazor.Client.Services.Contracts;
using GWTAI.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static GWTAI.Blazor.Client.Shared.Extensions.HttpClientExtensions;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
// Purpose?
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add services to the container.
//builder.Services.AddRazorComponents()
//    .AddInteractiveServerComponents();

// Add custom services
builder.Services.AddHttpClient<BookmarkService>(client =>
{
  client.ConfigureGWTAIServiceClient();
});


builder.Services.AddScoped(sp => new HttpClient
{
  BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Add custom services
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IAlertService, AlertService>();

builder.Services.AddSingleton<PageHistoryState>();

await builder.Build().RunAsync();
