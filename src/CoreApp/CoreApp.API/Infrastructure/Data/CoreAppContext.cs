using System.Data;
using System.Reflection;
using CoreApp.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreApp.API.Infrastructure.Data;

public class CoreAppContext : DbContext
{
  public CoreAppContext(DbContextOptions<CoreAppContext> options) : base(options) { }

  private IDbContextTransaction? _currentTransaction;

  public DbSet<Bookmark> Bookmarks { get; set; } = null!;
  public DbSet<BookmarkFolder> BookmarkFolders { get; set; } = null!;

  public DbSet<Person> Persons { get; init; } = null!;


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema("coreapp");

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  #region Transaction Handling
  public void BeginTransaction()
    {
        if (_currentTransaction != null)
        {
            return;
        }

        //if (!Database.IsInMemory())
        //{
        //    _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        //}
    }

    public void CommitTransaction()
    {
        try
        {
            _currentTransaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
    #endregion
}
