using CoreApp.API.Domain.Services;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.AiServices;
using CoreApp.API.Infrastructure.ExternalServices.ollama;
using CoreApp.API.Infrastructure.Security;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Wolverine;

namespace CoreApp.API;

public static class ServicesExtensions
{
  public static void AddCoreAppAPI(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddMediator(cfg =>
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
    );
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    services.AddScoped(
        typeof(IPipelineBehavior<,>),
        typeof(DBContextTransactionPipelineBehavior<,>)
    );

    services.AddFluentValidationAutoValidation();
    services.AddFluentValidationClientsideAdapters();
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    services.AddAutoMapper(typeof(Program));

    services.AddScoped<IPasswordHasher, PasswordHasher>();
    services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
    services.AddScoped<IProfileReader, ProfileReader>();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    services.AddHttpClient<IAIService2, AIService2>();

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

    // Configure Wolverine
    services.AddWolverine(opts =>
    {
      // Register the consumer in Wolverine. Wolverine will scan for Consumers by default.
      // If you explicitly want to register, you can do:
      // opts.Discovery.IncludeAssembly(typeof(ProcessUploadCommandConsumer).Assembly);

      // If using RabbitMQ
      opts.UseRabbitMq(mq =>
      {
        mq.HostName = "localhost"; // Or your RabbitMQ host
        mq.Port = 5672;
        mq.UserName = "guest";
        mq.Password = "guest";
      }).AutoProvision(); // Auto-provision queues and exchanges

      // Map message types to their queues (optional, but good practice for clarity)
      opts.PublishMessage<UploadRequested>().ToRabbitQueue("upload-requested-queue");
      opts.PublishMessage<UploadProcessingResult>().ToRabbitQueue("upload-processing-result-queue");

      // Configure the consumer to listen on the queue
      opts.ListenToRabbitQueue("upload-requested-queue")
          .ProcessMessagesWith<ProcessUploadCommandConsumer>(); // Explicitly define consumer for this queue

      // For results, you might listen on a different queue or use a different mechanism
      // if you want to update the client directly, e.g., SignalR or a webhook.
      // For now, let's assume another system will pick up UploadProcessingResult.
    });

  }

  public static void AddJwt(this IServiceCollection services)
  {
    services.AddOptions();

    var signingKey = new SymmetricSecurityKey(
        "somethinglongerforthisdumbalgorithmisrequired"u8.ToArray()
    );
    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    var issuer = "issuer";
    var audience = "audience";

    services.Configure<JwtIssuerOptions>(options =>
    {
      options.Issuer = issuer;
      options.Audience = audience;
      options.SigningCredentials = signingCredentials;
    });

    var tokenValidationParameters = new TokenValidationParameters
    {
      // The signing key must match!
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = signingCredentials.Key,
      // Validate the JWT Issuer (iss) claim
      ValidateIssuer = true,
      ValidIssuer = issuer,
      // Validate the JWT Audience (aud) claim
      ValidateAudience = true,
      ValidAudience = audience,
      // Validate the token expiry
      ValidateLifetime = true,
      // If you want to allow a certain amount of clock drift, set that here:
      ClockSkew = TimeSpan.Zero
    };

    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = tokenValidationParameters;
          options.Events = new JwtBearerEvents
          {
            OnMessageReceived = (context) =>
                {
                  var token = context.HttpContext.Request.Headers.Authorization.ToString();
                  if (token.StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                  {
                    context.Token = token["Token ".Length..].Trim();
                  }

                  return Task.CompletedTask;
                }
          };
        });
  }

  public static void AddSerilogLogging(this ILoggerFactory loggerFactory)
  {
    // Attach the sink to the logger configuration
    var log = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        //just for local debug
        .WriteTo.Console(
            outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {SourceContext} {Message}{NewLine}{Exception}",
            theme: AnsiConsoleTheme.Code
        )
        .CreateLogger();

    loggerFactory.AddSerilog(log);
    Log.Logger = log;
  }
}
