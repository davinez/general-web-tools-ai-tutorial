using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Tags;

public class List
{
    public record Query : IRequest<TagsEnvelope>;

    public class QueryHandler(CoreAppContext context) : IRequestHandler<Query, TagsEnvelope>
    {
        public async Task<TagsEnvelope> Handle(Query message, CancellationToken cancellationToken)
        {
            var tags = await context
                .Tags.OrderBy(x => x.TagId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return new TagsEnvelope
            {
                Tags = tags?.Select(x => x.TagId ?? string.Empty).ToList() ?? new List<string>()
            };
        }
    }
}
