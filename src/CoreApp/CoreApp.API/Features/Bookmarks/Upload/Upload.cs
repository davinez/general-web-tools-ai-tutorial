using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain;
using CoreApp.API.Features.Articles;
using CoreApp.API.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreApp.API.Features.Bookmarks.Upload;

public class Upload
{
  public record Command(UploadRequest File) : IRequest<UploadResponse>;

  public class UploadValidator : AbstractValidator<UploadRequest>
  {
    public UploadValidator()
    {
      RuleFor(x => x.FileName).NotNull().NotEmpty();
      RuleFor(x => x.FileContent).NotNull().NotEmpty();
      RuleFor(x => x.UploadTimestamp).NotNull().NotEmpty();
    }
  }

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator() =>
        RuleFor(x => x.File).NotNull().SetValidator(new UploadValidator());
  }

  public class Handler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor) : IRequestHandler<Command, UploadResponse>
  {
    public async Task<UploadResponse> Handle(
        Command command,
        CancellationToken cancellationToken
    )
    {
      // TODO: Map byte file to string and check type of file

      // TODO: Map to model of html content

      /* TODO: Processes content

      1- Remove duplicates
      2- Group by similars (create folders and add same page / content to that folder)
      3- Order
      4- Add to database (store folders and its corresponding bookmarks
      5- Add to database (store file content table / metrics)

      */

      var author = await context.Persons.FirstAsync(
          x => x.Username == currentUserAccessor.GetCurrentUsername(),
          cancellationToken
      );
      var tags = new List<Tag>();
      foreach (var tag in (message.Article.TagList ?? Enumerable.Empty<string>()))
      {
        var t = await context.Tags.FindAsync(tag);
        if (t == null)
        {
          t = new Tag { TagId = tag };
          await context.Tags.AddAsync(t, cancellationToken);
          //save immediately for reuse
          await context.SaveChangesAsync(cancellationToken);
        }
        tags.Add(t);
      }

      var article = new Article
      {
        Author = author,
        Body = message.Article.Body,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Description = message.Article.Description,
        Title = message.Article.Title,
        Slug = message.Article.Title.GenerateSlug()
      };
      await context.Articles.AddAsync(article, cancellationToken);

      await context.ArticleTags.AddRangeAsync(
          tags.Select(x => new ArticleTag { Article = article, Tag = x }),
          cancellationToken
      );

      await context.SaveChangesAsync(cancellationToken);

      return new ArticleEnvelope(article);
    }
  }
}
