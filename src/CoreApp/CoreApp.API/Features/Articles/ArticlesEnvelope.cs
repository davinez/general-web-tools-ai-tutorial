using System.Collections.Generic;
using CoreApp.API.Domain;

namespace CoreApp.API.Features.Articles;

public class ArticlesEnvelope
{
    public List<Article> Articles { get; set; } = new();

    public int ArticlesCount { get; set; }
}
