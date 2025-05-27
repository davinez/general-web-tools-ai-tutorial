using System;
using System.Collections.Generic;
using CoreApp.API.Domain.Common;

namespace CoreApp.API.Domain;

public class BookmarkFolder : BaseEntity
{
  public required string Title { get; set; }
  public DateTime? AddDate { get; set; }
  public DateTime? LastModified { get; set; }

  // Foreign Key
  public int? ParentFolderId { get; set; }
  public BookmarkFolder? ParentFolder { get; set; }

  // Relationships
  public List<Bookmark> Bookmarks { get; set; } = new();
  public List<BookmarkFolder> SubFolders { get; set; } = new();


}
