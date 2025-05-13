using GWTAI.Blazor.Client;
using GWTAI.Blazor.Client.Services;
using GWTAI.Blazor.Client.Services.Contracts;
using GWTAI.Blazor.Client.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


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


builder.Services.AddHttpClient("Local", (_, c) =>
{
  c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddHttpClient<BookmarkService>("CoreApi", (_, c) =>
{
  c.BaseAddress = new Uri("http://localhost:8081/");
  c.DefaultRequestHeaders.Add("Custom-Header", "MockHeaderValue");
});


// Add custom services
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IAlertService, AlertService>();

builder.Services.AddSingleton<PageHistoryState>();

await builder.Build().RunAsync();
