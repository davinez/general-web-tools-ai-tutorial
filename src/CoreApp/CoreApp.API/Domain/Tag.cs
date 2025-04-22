using System.Collections.Generic;

namespace CoreApp.API.Domain;

public class Tag
{
    public string? TagId { get; init; }

    public List<ArticleTag> ArticleTags { get; init; } = new();
}
