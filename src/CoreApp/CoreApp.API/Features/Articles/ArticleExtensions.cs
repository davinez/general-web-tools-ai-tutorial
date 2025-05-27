using System.Linq;
using CoreApp.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Articles;

public static class ArticleExtensions
{
    public static IQueryable<Article> GetAllData(this DbSet<Article> articles) =>
        articles
            .Include(x => x.Author)
            .Include(x => x.ArticleFavorites)
            .Include(x => x.ArticleTags)
            .AsNoTracking();
}
