using CoreApp.API;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Data.Interceptors;
using CoreApp.API.Infrastructure.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using StackExchange.Redis;
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

  builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
  builder.Services.AddScoped<ApplicationDbContextInitialiser>();

  var connectionString = builder.Configuration["ConnectionStrings:CoreAppDB"] ?? throw new CoreAppException("Missing connection string");
  var databaseProvider = builder.Configuration["ConnectionStrings:CoreAppDBProvider"] ?? throw new CoreAppException("Missing CoreAppDBProvider");
  var redisConnectionString = builder.Configuration["Redis:ConnectionString"] ?? throw new CoreAppException("Missing Redis:ConnectionString");
  var redsChannel = builder.Configuration["Redis:RedisChannel"] ?? throw new CoreAppException("Missing Redis:RedisChannel");

  builder.Services.AddDbContext<CoreAppContext>((sp, options) =>
  {
    if (databaseProvider.ToLowerInvariant().Trim().Equals("sqlite", StringComparison.Ordinal))
    {
      // options.UseSqlite(connectionString);
    }
    else if (
        databaseProvider.ToLowerInvariant().Trim().Equals("postgres", StringComparison.Ordinal)
    )
    {

      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

      options.UseNpgsql(connectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", "coreapp"));
    }
    else
    {
      throw new InvalidOperationException(
          "Database provider unknown. Please check configuration"
      );
    }
  });

  // if we're using the Interface, we register the interface
 // builder.Services.AddScoped<ICoreAppContext>(provider => provider.GetRequiredService<CoreAppContext>());

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddOpenApiDocument((configure, sp) =>
  {
    configure.Title = "GWTAI API";

    // Add JWT
    configure.AddSecurity("bearer", new NSwag.OpenApiSecurityScheme
    {
      Type = OpenApiSecuritySchemeType.ApiKey,
      Name = "Authorization",
      In = OpenApiSecurityApiKeyLocation.Header,
      Description = "Type into the textbox: Bearer {your JWT token}."
    });

    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));

  });

  builder.Services.AddCors();
  builder.Services.AddCoreAppAPI(builder.Configuration);
  builder.Services.AddValidators();

  builder.Services.AddJwt();

  // Hubs
  // Add SignalR services
  builder.Services.AddSignalR()
      .AddStackExchangeRedis(redisConnectionString, options =>
      {
        options.Configuration.ChannelPrefix = RedisChannel.Literal(redsChannel);
      });

  // App

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  app.UseSerilogRequestLogging(); // Optional: Log all HTTP requests

  app.UseMiddleware<ErrorHandlingMiddleware>();

  app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

  app.UseAuthentication();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.InitialiseDatabase();
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


  // Map the SignalR hub
  app.MapHub<JobEventStatusHub>("/jobEventStatusHub");


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
