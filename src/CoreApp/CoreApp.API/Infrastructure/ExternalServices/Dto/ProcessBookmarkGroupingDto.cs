using System;

namespace CoreApp.API.Infrastructure.ExternalServices.Dto;

public class ProcessBookmarkGroupingDto
{
  public required string Title { get; set; }
  public required string Url { get; set; }
  public SuggestionsAI? SuggestionsAI { get; set; }
  public required CurrentStructure CurrentStructure { get; set; }
}

public class CurrentStructure
{
  public required string RouteIds { get; set; }
  public required string RouteNames { get; set; }
}

public class SuggestionsAI
{
  public string? Route { get; set; }
  public string[] PossibleDuplicate { get; set; } = Array.Empty<string>();
}
