using System;

namespace CoreApp.API.Infrastructure.Hubs.Dto;

public class JobEventStatusHubDto
{
  public required string JobId { get; set; }
  public required string Status { get; set; }
  public DateTime Timestamp { get; set; }
}
