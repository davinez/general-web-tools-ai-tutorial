using CoreApp.API.Domain.Security;
using CoreApp.API.Domain.Services.ExternalServices;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.ExternalServices.AiServices;
using CoreApp.API.Infrastructure.ExternalServices.Storage;
using CoreApp.API.Infrastructure.Security;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Wolverine;

namespace CoreApp.API.Config.Extensions;

public static class APIExtensions
{
  public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
  {
    bool isDevelop = environment.IsDevelopment();

    services.AddCors(options =>
    {
      options.AddPolicy(name: "gwtai_policy",
                        policy =>
                        {
                          if (isDevelop)
                          {
                            policy.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader();
                          }
                          else
                          {
                            policy
                                   .SetIsOriginAllowedToAllowWildcardSubdomains()
                                   .WithOrigins("https://*.davidnez.work", "http://localhost:5173")
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                          }
                        });
    });

    services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

    services.AddHttpContextAccessor();

    //services.AddHealthChecks()
    //    .AddDbContextCheck<ManagerContext>();

    services.AddExceptionHandler<CustomExceptionHandler>();

    services.AddEndpointsApiExplorer();

    services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

    services.AddControllers();

    // Customise default API behaviour
    services.Configure<ApiBehaviorOptions>(options =>
        options.SuppressModelStateInvalidFilter = true);

    return services;
  }

  public static void AddOpenAPI(this IServiceCollection services)
  {
    services.AddOpenApiDocument((configure, sp) =>
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
  }

  public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
  {
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())

    return app;
  }

  public static void AddAuth(this IServiceCollection services)
  {
    //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //     .AddMicrosoftIdentityWebApi(options =>
    //     {

    //       configuration.Bind("EntraIDAuthConfig", options);
    //       options.Events = new JwtBearerEvents();

    //       // The following lines code instruct the asp.net core middleware to use the data in the "roles" claim in the [Authorize] attribute, policy.RequireRole() and User.IsInRole()
    //       // See https://docs.microsoft.com/aspnet/core/security/authorization/roles for more info.
    //       // https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/blob/master/5-WebApp-AuthZ/5-2-Groups/Startup.cs
    //       // options.TokenValidationParameters.RoleClaimType = "groups";

    //       /// <summary>
    //       /// Below you can do extended token validation and check for additional claims, such as:
    //       ///
    //       /// - check if the caller's tenant is in the allowed tenants list via the 'tid' claim (for multi-tenant applications)
    //       /// - check if the caller's account is homed or guest via the 'acct' optional claim
    //       /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
    //       ///
    //       /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
    //       /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
    //       /// </summary>

    //       //options.Events.OnTokenValidated = async context =>
    //       //{
    //       //    string[] allowedClientApps = { /* list of client ids to allow */ };

    //       //    string clientappId = context?.Principal?.Claims
    //       //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

    //       //    if (!allowedClientApps.Contains(clientappId))
    //       //    {
    //       //        throw new System.Exception("This client is not authorized");
    //       //    }
    //       //};
    //     }, options => { configuration.Bind("EntraIDAuthConfig", options); }, "Bearer", true);

    //services.AddAuthorization(options =>
    //{
    //  //options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
    //  //options.AddPolicy("General.Level1", policy => policy.RequireRole("General.Level1"));

    //  options.AddPolicy("All", policy =>
    //  {
    //    policy.RequireAssertion(context =>
    //                 context.User.HasClaim(ClaimTypes.Role, "Administrator") ||
    //                 context.User.HasClaim(ClaimTypes.Role, "General.Level1"));

    //    string[] scopesClaim = configuration.GetSection("EntraIDAuthConfig:Scopes").Get<string[]>() ?? throw new ArgumentNullException($"Null value for Scopes in {nameof(CollectionGroups)}");

    //    // All scopes are required, not only one of allowedValues
    //    foreach (var scopeClaim in scopesClaim)
    //    {
    //      policy.RequireScope(scopeClaim);
    //    }
    //  });

    //});
  }

}
