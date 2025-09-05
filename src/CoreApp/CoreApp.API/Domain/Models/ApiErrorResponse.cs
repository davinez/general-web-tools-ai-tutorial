using System.Collections.Generic;

namespace CoreApp.API.Domain.Models;

public class ApiErrorResponse
{
  public required string ApiVersion { get; set; }
  public ApiTopLevelError Error { get; set; } = new ApiTopLevelError();
}

public class ApiTopLevelError
{
  public int Code { get; set; }
  public string? Message { get; set; }
  public List<ApiError> Errors { get; set; } = new List<ApiError>();
}

public class ApiError
{
  public string? Domain { get; set; }
  public string? Reason { get; set; }
  public string? Message { get; set; }
}
