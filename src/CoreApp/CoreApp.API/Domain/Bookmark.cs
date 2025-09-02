using System;
using CoreApp.API.Domain.Common;

namespace CoreApp.API.Domain;

public class Bookmark : BaseEntity
{
  public required string Title { get; set; }
  public required string Url { get; set; }
  public DateTimeOffset? AddDate { get; set; }
  public string? Icon { get; set; }

  // Foreign Key
  public int BookmarkFolderId { get; set; }
  public BookmarkFolder BookmarkFolder { get; set; } = null!;
}
