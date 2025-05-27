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

  public DbSet<Article> Articles { get; init; } = null!;
  public DbSet<Comment> Comments { get; init; } = null!;
  public DbSet<Person> Persons { get; init; } = null!;
  public DbSet<Tag> Tags { get; init; } = null!;
  public DbSet<ArticleTag> ArticleTags { get; init; } = null!;
  public DbSet<ArticleFavorite> ArticleFavorites { get; init; } = null!;
  public DbSet<FollowedPeople> FollowedPeople { get; init; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema("coreapp");

    modelBuilder.Entity<ArticleTag>(b =>
    {
      b.HasKey(t => new { t.ArticleId, t.TagId });

      b.HasOne(pt => pt.Article)
          .WithMany(p => p.ArticleTags)
          .HasForeignKey(pt => pt.ArticleId);

      b.HasOne(pt => pt.Tag).WithMany(t => t.ArticleTags).HasForeignKey(pt => pt.TagId);
    });

    modelBuilder.Entity<ArticleFavorite>(b =>
    {
      b.HasKey(t => new { t.ArticleId, t.PersonId });

      b.HasOne(pt => pt.Article)
          .WithMany(p => p.ArticleFavorites)
          .HasForeignKey(pt => pt.ArticleId);

      b.HasOne(pt => pt.Person)
          .WithMany(t => t.ArticleFavorites)
          .HasForeignKey(pt => pt.PersonId);
    });

    modelBuilder.Entity<FollowedPeople>(b =>
    {
      b.HasKey(t => new { t.ObserverId, t.TargetId });

      // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider,
      // app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated(); throws the following error:
      // System.Data.SqlClient.SqlException
      // HResult = 0x80131904
      // Message = Introducing FOREIGN KEY constraint 'FK_FollowedPeople_Persons_TargetId' on table 'FollowedPeople' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
      // Could not create constraint or index. See previous errors.
      b.HasOne(pt => pt.Observer)
          .WithMany(p => p.Followers)
          .HasForeignKey(pt => pt.ObserverId)
          .OnDelete(DeleteBehavior.Restrict);

      // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider,
      // app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated(); throws the following error:
      // System.Data.SqlClient.SqlException
      // HResult = 0x80131904
      // Message = Introducing FOREIGN KEY constraint 'FK_FollowingPeople_Persons_TargetId' on table 'FollowedPeople' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
      // Could not create constraint or index. See previous errors.
      b.HasOne(pt => pt.Target)
          .WithMany(t => t.Following)
          .HasForeignKey(pt => pt.TargetId)
          .OnDelete(DeleteBehavior.Restrict);
    });


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
