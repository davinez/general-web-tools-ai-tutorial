using System;


namespace CoreApp.API.features.Bookmarks.Dtos;

public class BookmarkDto
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Url { get; set; }
  public DateTime? AddDate { get; set; }
  public string? Icon { get; set; }
}
