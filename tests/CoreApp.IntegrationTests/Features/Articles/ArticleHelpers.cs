using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreApp.API.Domain;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Features.Articles;
using CoreApp.API.Infrastructure.Errors;
using CoreApp.IntegrationTests.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.IntegrationTests.Features.Articles;

public static class ArticleHelpers
{
  /// <summary>
  /// creates an article based on the given Create command. It also creates a default user
  /// </summary>
  /// <param name="fixture"></param>
  /// <param name="command"></param>
  /// <returns></returns>
  public static async Task<Article> CreateArticle(
      SliceFixture fixture,
      Create.Command command
  )
  {
    // first create the default user
    var user = await UserHelpers.CreateDefaultUser(fixture);
    if (user.Username is null)
    {
      throw new RestException(HttpStatusCode.BadRequest);
    }

    var dbContext = fixture.GetDbContext();
    var currentAccessor = new StubCurrentUserAccessor(user.Username);

    var articleCreateHandler = new Create.Handler(dbContext, currentAccessor);
    var created = await articleCreateHandler.Handle(
        command,
        new System.Threading.CancellationToken()
    );

    var dbArticle = await fixture.ExecuteDbContextAsync(db =>
        db.Articles.Where(a => a.ArticleId == created.Article.ArticleId).SingleOrDefaultAsync()
    );
    if (dbArticle is null)
    {
      throw new RestException(HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
    }

    return dbArticle;
  }
}
