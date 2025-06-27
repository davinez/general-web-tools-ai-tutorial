using System.Threading;
using System.Threading.Tasks;
using CoreApp.API.Domain;
using CoreApp.API.Features.Bookmarks.Dtos;
using CoreApp.API.Infrastructure;
using CoreApp.API.Infrastructure.Data;
using FluentValidation;
using MediatR;

namespace CoreApp.API.Features.Bookmarks.CreateFolders
{

  public record CreateFoldersCommand(CreateFoldersRequest Request) : IRequest;


  public class CreateFoldersCommandHandler
  {
    public class CreateFoldersValidator : AbstractValidator<CreateFoldersRequest>
    {
      public CreateFoldersValidator()
      {
        RuleFor(x => x.Folders).NotNull().NotEmpty();
      }
    }

    public class CommandValidator : AbstractValidator<CreateFoldersCommand>
    {
      public CommandValidator() =>
          RuleFor(x => x.Request).NotNull().SetValidator(new CreateFoldersValidator());
    }


    public class Handler : IRequestHandler<CreateFoldersCommand>
    {
      private readonly CoreAppContext _context;
      private readonly ICurrentUserAccessor _currentUserAccessor;


      public Handler(CoreAppContext context, ICurrentUserAccessor currentUserAccessor)
      {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
      }

      public async Task Handle(
        CreateFoldersCommand command,
        CancellationToken cancellationToken
       )
      {

        // Save in DB
        foreach (var folder in command.Request.Folders)
        {
          await SaveFoldersAndBookmarksToDatabase(folder, 0, _context, cancellationToken);
        }

      }

      private static async Task SaveFoldersAndBookmarksToDatabase(
                                   BookmarkFolderDto folder,
                                   int fatherId,
                                   CoreAppContext context,
                                   CancellationToken cancellationToken
                                 )
      {
        // Save the folder
        var dbFolder = new BookmarkFolder
        {
          Title = folder.Title,
          AddDate = folder.AddDate,
          ParentFolderId = fatherId,
        };

        await context.BookmarkFolders.AddAsync(dbFolder, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // Save the bookmarks in the folder
        foreach (var bookmark in folder.Bookmarks)
        {
          var dbBookmark = new Bookmark
          {
            Title = bookmark.Title,
            Url = bookmark.Url,
            AddDate = bookmark.AddDate,
            Icon = bookmark.Icon,
            BookmarkFolderId = dbFolder.Id,
          };

          await context.Bookmarks.AddAsync(dbBookmark, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        // Recursively save subfolders
        foreach (var subFolder in folder.SubFolders)
        {
          await SaveFoldersAndBookmarksToDatabase(subFolder, dbFolder.Id, context, cancellationToken);
        }
      }



    }





  }


}
