using CoreApp.API.features.Bookmarks.Dtos;
using CoreApp.API.Features.Bookmarks.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreApp.API.Utils;

/// <summary>
/// Generates a browser-importable HTML file from bookmark DTOs.
/// </summary>
public class BookmarkHtmlGenerator
{
  private readonly StringBuilder _stringBuilder;

  public BookmarkHtmlGenerator()
  {
    _stringBuilder = new StringBuilder();
  }

  /// <summary>
  /// Converts a list of root folders and bookmarks into an HTML string.
  /// </summary>
  /// <param name="rootFolders">A list of top-level bookmark folders.</param>
  /// <param name="rootBookmarks">A list of top-level bookmarks not inside any folder.</param>
  /// <returns>An HTML string compatible with browser bookmark importers.</returns>
  public string Generate(List<BookmarkFolderDto> rootFolders, List<BookmarkDto> rootBookmarks)
  {
    // 1. Append the standard HTML header
    _stringBuilder.AppendLine("<!DOCTYPE NETSCAPE-Bookmark-file-1>");
    _stringBuilder.AppendLine("<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">");
    _stringBuilder.AppendLine("<TITLE>Bookmarks</TITLE>");
    _stringBuilder.AppendLine("<H1>Bookmarks</H1>");
    _stringBuilder.AppendLine();
    _stringBuilder.AppendLine("<DL><p>");

    // 2. Process all top-level folders and bookmarks
    foreach (var folder in rootFolders)
    {
      BuildFolderHtml(folder, 1); // Start with an indent level of 1
    }

    foreach (var bookmark in rootBookmarks)
    {
      BuildBookmarkHtml(bookmark, 1);
    }

    // 3. Append the standard footer
    _stringBuilder.AppendLine("</DL><p>");

    return _stringBuilder.ToString();
  }

  /// <summary>
  /// Recursively builds the HTML for a folder and its contents.
  /// </summary>
  private void BuildFolderHtml(BookmarkFolderDto folder, int indentLevel)
  {
    var indent = new string(' ', indentLevel * 4); // For readability

    _stringBuilder.Append($"{indent}<DT><H3");
    if (folder.AddDate.HasValue)
    {
      _stringBuilder.Append($" ADD_DATE=\"{ToUnixTimestamp(folder.AddDate.Value)}\"");
    }
    _stringBuilder.AppendLine($">{WebUtility.HtmlEncode(folder.Title)}</H3>");

    _stringBuilder.AppendLine($"{indent}<DL><p>");

    // Recursively build sub-folders
    foreach (var subFolder in folder.SubFolders)
    {
      BuildFolderHtml(subFolder, indentLevel + 1);
    }

    // Build bookmarks within this folder
    foreach (var bookmark in folder.Bookmarks)
    {
      BuildBookmarkHtml(bookmark, indentLevel + 1);
    }

    _stringBuilder.AppendLine($"{indent}</DL><p>");
  }

  /// <summary>
  /// Builds the HTML for a single bookmark link.
  /// </summary>
  private void BuildBookmarkHtml(BookmarkDto bookmark, int indentLevel)
  {
    var indent = new string(' ', indentLevel * 4);

    _stringBuilder.Append($"{indent}<DT><A HREF=\"{WebUtility.HtmlEncode(bookmark.Url)}\"");

    if (bookmark.AddDate.HasValue)
    {
      _stringBuilder.Append($" ADD_DATE=\"{ToUnixTimestamp(bookmark.AddDate.Value)}\"");
    }
    if (!string.IsNullOrWhiteSpace(bookmark.Icon))
    {
      _stringBuilder.Append($" ICON=\"{bookmark.Icon}\"");
    }

    _stringBuilder.AppendLine($">{WebUtility.HtmlEncode(bookmark.Title)}</A>");
  }

  /// <summary>
  /// Converts a DateTime object to a Unix timestamp (seconds since epoch).
  /// </summary>
  private long ToUnixTimestamp(DateTime dateTime)
  {
    return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
  }
}
