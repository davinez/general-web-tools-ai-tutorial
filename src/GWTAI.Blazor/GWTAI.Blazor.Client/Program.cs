using GWTAI.Blazor.Client;
using GWTAI.Blazor.Client.Services;
using GWTAI.Blazor.Client.Services.Contracts;
using GWTAI.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using static GWTAI.Blazor.Client.Shared.Extensions.HttpClientExtensions;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Get Config Values
string? coreAppApiUrl = builder.Configuration.GetSection("CoreAppApiUrl").Value;
ArgumentNullException.ThrowIfNull(coreAppApiUrl);



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

// CoreApp URL
builder.Services.AddScoped(sp => {
  var apiUrl = new Uri("https://localhost:5001");
  return new HttpClient() { BaseAddress = apiUrl };
});

// Base address of the Blazor WebAssembly app itself,
// for making requests within the same domain where the Blazor WebAssembly app is hosted.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(coreAppApiUrl) });


// Add custom services
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IAlertService, AlertService>();

builder.Services.AddSingleton<PageHistoryState>();

await builder.Build().RunAsync();
