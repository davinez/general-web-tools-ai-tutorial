using System.Linq;
using System.Threading.Tasks;
using CoreApp.API.Features.Articles;
using Xunit;

namespace CoreApp.IntegrationTests.Features.Articles;

public class CreateTests : SliceFixture
{
  [Fact]
  public async Task Expect_Create_Article()
  {
    var command = new Create.Command(
        new Create.ArticleData
        {
          Title = "Test article dsergiu77",
          Description = "Description of the test article",
          Body = "Body of the test article",
          TagList = ["tag1", "tag2"]
        }
    );

    var article = await ArticleHelpers.CreateArticle(this, command);

    Assert.NotNull(article);
    Assert.Equal(article.Title, command.Article.Title);
    Assert.Equal(article.TagList.Count(), command.Article.TagList?.Count());
  }
}
