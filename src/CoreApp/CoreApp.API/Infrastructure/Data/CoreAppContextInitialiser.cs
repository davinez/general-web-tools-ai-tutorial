using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CoreApp.API.Infrastructure.Data;

public static class InitialiserExtensions
{
  public static async Task InitialiseDatabaseAsync(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();

    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

    await initialiser.InitialiseAsync();

    // await initialiser.SeedAsync();
  }
}

public class ApplicationDbContextInitialiser
{
  private readonly ILogger<ApplicationDbContextInitialiser> _logger;
  private readonly CoreAppContext _context;

  public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, CoreAppContext context)
  {
    _logger = logger;
    _context = context;
  }

  public async Task InitialiseAsync()
  {
    try
    {
      if ((await _context.Database.GetPendingMigrationsAsync()).Any())
      {
        await _context.Database.MigrateAsync();
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred while initialising the database.");
      throw;
    }
  }

  //public async Task SeedAsync()
  //{
  //  try
  //  {
  //    await TrySeedAsync();
  //  }
  //  catch (Exception ex)
  //  {
  //    _logger.LogError(ex, "An error occurred while seeding the database.");
  //    throw;
  //  }
  //}


}
