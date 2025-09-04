using CoreApp.API.Domain.Errors.Exceptions;
using CoreApp.API.Domain.MessageBrokers.Producers;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Data.Interceptors;
using CoreApp.API.Infrastructure.ExternalServices.AiServices;
using CoreApp.API.Infrastructure.ExternalServices.Storage;
using CoreApp.API.Infrastructure.MessageBrokers.Dto;
using CoreApp.API.Infrastructure.MessageBrokers.Producers;
using CoreApp.API.Infrastructure.Security;
using Mediator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using Wolverine;
using Wolverine.RabbitMQ;

namespace CoreApp.API.Config.Extensions
{
  public static class InfrastructureExtensions
  {

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration["ConnectionStrings:CoreAppDB"] ?? throw new CoreAppException("Missing connection string");
      var databaseProvider = configuration["ConnectionStrings:CoreAppDBProvider"] ?? throw new CoreAppException("Missing CoreAppDBProvider");
 
      services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
      services.AddScoped<ApplicationDbContextInitialiser>();

      services.AddDbContext<CoreAppContext>((sp, options) =>
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

      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
      services.AddScoped(
          typeof(IPipelineBehavior<,>),
          typeof(DBContextTransactionPipelineBehavior<,>)
      );

      // Register AI Service based on configuration
      var aiServiceProvider = configuration["AiService:Provider"];
      switch (aiServiceProvider)
      {
        case "Gemini":
          services.AddHttpClient<IAiService, GeminiAiService>(client =>
          {
            client.BaseAddress = new Uri("https://generativelanguage.googleapis.com");
          });
          break;
        case "AzureOpenAI":
          services.AddScoped<IAiService, AzureOpenAiService>();
          break;
        default:
          throw new InvalidOperationException("Invalid AI Service Provider specified in configuration.");
      }

      // Register Storage Service based on configuration
      var storageServiceProvider = configuration["StorageService:Provider"];
      switch (storageServiceProvider)
      {
        case "S3":
          services.AddScoped<IStorageService, S3StorageService>();
          break;
        case "AzureBlobStorage":
          services.AddScoped<IStorageService, AzureBlobStorageService>();
          break;
        default:
          throw new InvalidOperationException("Invalid Storage Service Provider specified in configuration.");
      }

      // Configure Wolverine
      services.AddWolverine(opts =>
      {
        // Register the consumer in Wolverine. Wolverine will scan for Consumers by default.
        // If you explicitly want to register, you can do:
        // opts.Discovery.IncludeAssembly(typeof(ProcessUploadCommandConsumer).Assembly);

        // If using RabbitMQ
        opts.UseRabbitMq(mq =>
        {
          mq.HostName = "localhost"; 
          mq.Port = 5672;
          mq.UserName = "guest";
          mq.Password = "guest";
        }).AutoProvision(); // Auto-provision queues and exchanges

        // Map message types to their queues (optional, but good practice for clarity)
        opts.PublishMessage<UploadBookmarksMessageRequest>().ToRabbitQueue("upload-requested-queue");
        opts.PublishMessage<DeleteBookmarksMessageRequest>().ToRabbitQueue("upload-processing-result-queue");

        // Configure the consumer to listen on the queue
        opts.ListenToRabbitQueue("upload-requested-queue");
        //.ProcessMessagesWith<UploadBookmarksMessageConsumer>(); // Explicitly define consumer for this queue

        // For results, you might listen on a different queue or use a different mechanism
        // if you want to update the client directly, e.g., SignalR or a webhook.
        // For now, let's assume another system will pick up UploadProcessingResult.
      });

      services.AddScoped<IBookmarksMessageProducer, BookmarksMessageProducer>();

      var redisEndPoint = configuration["Redis:Endpoint"] ?? throw new CoreAppException("Missing Redis:Endpoint");
      var redsUser = configuration["Redis:User"] ?? throw new CoreAppException("Missing Redis:User");
      var redisPassword = configuration["Redis:Password"] ?? throw new CoreAppException("Missing Redis:Password");
      var redisChannel = configuration["Redis:RedisChannel"] ?? throw new CoreAppException("Missing Redis:RedisChannel");

      var redisConfiguration = new ConfigurationOptions
      {     
        EndPoints = { redisEndPoint },
        User = redsUser,
        Password = redisPassword,
        ChannelPrefix = RedisChannel.Literal(redisChannel)
        //,Ssl = true
        // options.Configuration.SyncTimeout = 5000; // 5 seconds
        // options.Configuration.ConnectTimeout = 5000; // 5 seconds
        // options.Configuration.KeepAlive = 180; // 3 minutes
        // options.Configuration.ReconnectRetryPolicy = new ExponentialRetry(5000); // Retry every 5 seconds
      };

      // Hubs
      // Add SignalR services
      services.AddSignalR().AddStackExchangeRedis(options => { options.Configuration = redisConfiguration; });

      return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, IWebHostEnvironment environment, ConfigurationManager configuration)
    {
      var keyVaultUri = configuration["KEY_VAULT_PROD_ENDPOINT"];

      if (environment.IsProduction()
          && !string.IsNullOrWhiteSpace(keyVaultUri))
      {
        // We need the environment variables:
        // AZURE_CLIENT_ID,
        // AZURE_TENANT_ID,
        // AZURE_CLIENT_SECRET
        // to make sure the DefaultAzureCredential will work.


        //configuration.AddAzureKeyVault(
        //    new Uri(keyVaultUri),
        //    new DefaultAzureCredential());
      }

      return services;
    }
  }
}
