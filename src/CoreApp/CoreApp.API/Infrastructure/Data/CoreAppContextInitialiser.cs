using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CoreApp.API.Domain.Errors;

namespace CoreApp.API.Infrastructure.Data;

public static class InitialiserExtensions
{
  public static void InitialiseDatabase(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();

    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

    initialiser.Initialise();

    // await initialiser.Seed();
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

  public void Initialise()
  {
    try
    {
      if (!_context.Database.CanConnect())
      {
        throw new CoreAppException("Database cannot connect");
      }

      var pendingMigrations = _context.Database.GetPendingMigrations();

      if (pendingMigrations.Any())
      {
        _context.Database.Migrate();
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred while initialising the database.");
      throw;
    }
  }

  //public void Seed()
  //{
  //  try
  //  {
  //    TrySeed();
  //  }
  //  catch (Exception ex)
  //  {
  //    _logger.LogError(ex, "An error occurred while seeding the database.");
  //    throw;
  //  }
  //}


}
