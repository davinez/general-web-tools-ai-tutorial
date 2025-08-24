using CoreApp.API.Infrastructure.Data;
using CoreApp.API.Infrastructure.MessageBrokers.Dto;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.MessageBrokers.Consumers;

public class DeleteBookmarksMessageConsumer
{
  private readonly ILogger<DeleteBookmarksMessageConsumer> _logger;
  private readonly CoreAppContext _context;

  public DeleteBookmarksMessageConsumer(ILogger<DeleteBookmarksMessageConsumer> logger, CoreAppContext context)
  {
    _logger = logger;
    _context = context;
  }

  public async Task Consume(DeleteBookmarksMessageRequest message, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Processing delete bookmarks request for Bookmark: {BookmarkId}",
                          message.BookmarkId);
    try
    {
      // Logic to delete bookmarks from the database
      await Task.Delay(30); // Simulate deletion work

      _logger.LogInformation("Bookmarks deleted for Bookmark: {BookmarkId}", message.BookmarkId);
      // You could cascade an event here too, e.g., BookmarksDeletedEvent
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error deleting bookmarks for Bookmark: {BookmarkId}", message.BookmarkId);
    }
  }

}
