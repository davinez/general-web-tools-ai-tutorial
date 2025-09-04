using CoreApp.API.Domain.Security;
using CoreApp.API.Endpoints.Bookmarks.CreateFolders;
using CoreApp.API.Endpoints.Bookmarks.Upload;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Security;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace CoreApp.API.Config.Extensions;

public static class ApplicationExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddMediator((options) =>
    {
      //options.Namespace = "SimpleConsole.Mediator";
      options.ServiceLifetime = ServiceLifetime.Scoped;
      // Only available from v3:
      options.GenerateTypesAsInternal = true;
      options.NotificationPublisherType = typeof(ForeachAwaitPublisher);
      //options.Assemblies = [];
      //options.PipelineBehaviors = [];
      //options.StreamPipelineBehaviors = [];
    });

    services.AddSingleton(TimeProvider.System);
    services.AddScoped<IValidator<UploadCommand>, UploadCommandHandler.CommandValidator>();
    services.AddScoped<IValidator<CreateFoldersCommand>, CreateFoldersCommandHandler.CommandValidator>();

    services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

    // Create and configure the options object
    var jsonSerializerOptions = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      // Add any other settings you need
      // WriteIndented = true, 
    };

    // Register it as a singleton
    services.AddSingleton(jsonSerializerOptions);

    return services;
  }
}
