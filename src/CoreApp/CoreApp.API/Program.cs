using CoreApp.API.Config.Extensions;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System;

// Configure Serilog for bootstrap logging. This will catch errors during startup.
// This should be the very first thing in your Program.cs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
  Log.Information("Starting web application");

  var builder = WebApplication.CreateBuilder(args);

  // Configure Serilog as the logging provider for the application
  builder.Host.UseSerilog((context, services, configuration) => configuration
      .ReadFrom.Configuration(context.Configuration)
      .ReadFrom.Services(services)
      .Enrich.FromLogContext()
      .WriteTo.Console());

  // Add services to the container.
  builder.Logging.AddLoggingServices(builder.Configuration, builder.Environment);
  builder.Services.AddKeyVaultIfConfigured(builder.Environment, builder.Configuration);
  builder.Services.AddAuth();
  builder.Services.AddOpenAPI();
  builder.Services.AddApplicationServices(builder.Configuration);
  builder.Services.AddInfrastructureServices(builder.Configuration);
  builder.Services.AddAPIServices(builder.Configuration, builder.Environment);

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.InitialiseDatabase();
    app.UseDefaultOpenApi();
    IdentityModelEventSource.ShowPII = false;
  }
  else if (app.Environment.IsProduction())
  {
    // For prod environment, ideally we want a script migration not auto-migration using EF Core
    // example dotnet ef migrations script --idempotent --output EFCore/migrations.sql
    app.InitialiseDatabase();
    app.UseOpenApi();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }
  else
  {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  // Configure the HTTP request pipeline.
  app.UseSerilogRequestLogging(); // Optional: Log all HTTP requests
 // app.UseHealthChecks("/health");
  //app.UseHttpsRedirection();
  app.UseCors("gwtai_policy");
  app.UseAuthentication();
  app.UseAuthorization();


  app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

  // app.Map("/", () => Results.Redirect("/api"));

  // Map the SignalR hub
  app.MapHub<JobEventStatusHub>("/jobEventStatusHub");

  app.UseExceptionHandler(options => { });


  app.Run();

}
catch (Exception ex) when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design") // see https://github.com/dotnet/efcore/issues/29923
{
  Log.Fatal(ex, "Web host terminated unexpectedly");
  // return 1;
}
finally
{
  // IMPORTANT: This flushes any buffered logs and gracefully shuts down Serilog
  Log.CloseAndFlush();
}
// return 0;
