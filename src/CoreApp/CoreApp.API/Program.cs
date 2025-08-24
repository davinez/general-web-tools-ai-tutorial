using CoreApp.API;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Hubs;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.Data.Interceptors;
using CoreApp.API.Infrastructure.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
builder.Services.AddScoped<ApplicationDbContextInitialiser>();

builder.Services.AddSingleton(TimeProvider.System);

var connectionString = builder.Configuration["ConnectionStrings:CoreAppDB"] ?? throw new CoreAppException("Missing connection string");
var databaseProvider = builder.Configuration["ConnectionStrings:CoreAppDBProvider"] ?? throw new CoreAppException("Missing CoreAppDBProvider");

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
// builder.Services.AddScoped<CoreAppContext>(provider => provider.GetRequiredService<CoreAppContext>());

builder.Services.AddLocalization(x => x.ResourcesPath = "Resources");

// Inject an implementation of ISwaggerProvider with defaulted settings applied
builder.Services.AddSwaggerGen(x =>
{
  x.AddSecurityDefinition(
      "Bearer",
      new OpenApiSecurityScheme
      {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT"
      }
  );

  x.SupportNonNullableReferenceTypes();

  x.AddSecurityRequirement(
      new OpenApiSecurityRequirement
      {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
      }
  );
  x.SwaggerDoc("v1", new OpenApiInfo { Title = "GWTAI Core API", Version = "v1" });
  x.CustomSchemaIds(y => y.FullName);
  x.DocInclusionPredicate((_, _) => true);
  x.TagActionsBy(y => new List<string> { y.GroupName ?? throw new InvalidOperationException() });
  x.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
});

builder.Services.AddCors();
builder
    .Services.AddMvc(opt =>
    {
      opt.Conventions.Add(new GroupByApiRootConvention());
      opt.Filters.Add(typeof(ValidatorActionFilter));
      opt.EnableEndpointRouting = false;
    })
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.DefaultIgnoreCondition = System
            .Text
            .Json
            .Serialization
            .JsonIgnoreCondition
            .WhenWritingNull
    );

builder.Services.AddCoreAppAPI(builder.Configuration);

builder.Services.AddJwt();

// Hubs
// Add SignalR services
builder.Services.AddSignalR()
    .AddStackExchangeRedis("your_redis_connection_string", options => {
      options.Configuration.ChannelPrefix = RedisChannel.Literal("YourApp");
    });



// App

var app = builder.Build();

app.Services.GetRequiredService<ILoggerFactory>().AddSerilogLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseMvc();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");

// Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "RealWorld API V1"));


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  await app.InitialiseDatabaseAsync();
  IdentityModelEventSource.ShowPII = false;
}
else if (app.Environment.IsProduction())
{
  // For prod environment, ideally we want a script migration not auto-migration using EF Core
  // example dotnet ef migrations script --idempotent --output EFCore/migrations.sql
  await app.InitialiseDatabaseAsync();
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
