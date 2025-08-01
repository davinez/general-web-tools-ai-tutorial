using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreApp.API.Domain.Errors;
using CoreApp.API.Features.Comments;
using CoreApp.API.Infrastructure.Errors;
using CoreApp.IntegrationTests.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.IntegrationTests.Features.Comments;

public static class CommentHelpers
{
  /// <summary>
  /// creates an article comment based on the given Create command.
  /// Creates a default user if parameter userName is empty.
  /// </summary>
  /// <param name="fixture"></param>
  /// <param name="command"></param>
  /// <param name="userName"></param>
  /// <returns></returns>
  public static async Task<API.Domain.Comment> CreateComment(
      SliceFixture fixture,
      Create.Command command,
      string userName
  )
  {
    if (string.IsNullOrWhiteSpace(userName))
    {
      var user = await UserHelpers.CreateDefaultUser(fixture);

      if (user.Username is null)
      {
        throw new RestException(HttpStatusCode.BadRequest);
      }

      userName = user.Username;
    }

    var dbContext = fixture.GetDbContext();
    var currentAccessor = new StubCurrentUserAccessor(userName);

    var commentCreateHandler = new Create.Handler(dbContext, currentAccessor);
    var created = await commentCreateHandler.Handle(
        command,
        new System.Threading.CancellationToken()
    );

    var dbArticleWithComments = await fixture.ExecuteDbContextAsync(db =>
        db.Articles.Include(a => a.Comments)
            .Include(a => a.Author)
            .Where(a => a.Slug == command.Slug)
            .SingleOrDefaultAsync()
    );

    if (dbArticleWithComments is null)
    {
      throw new RestException(HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
    }

    var dbComment = dbArticleWithComments.Comments.FirstOrDefault(c =>
        c.ArticleId == dbArticleWithComments.ArticleId
        && c.Author == dbArticleWithComments.Author
    );

    if (dbComment is null)
    {
      throw new RestException(HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
    }

    return dbComment;
  }
}
