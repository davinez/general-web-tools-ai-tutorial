using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CoreApp.API.Config.Extensions
{
  public static class LoggingExtensions
  {
    public static ILoggingBuilder AddLoggingServices(this ILoggingBuilder logging, IConfiguration configuration, IWebHostEnvironment environment)
    {
      

      return logging;
    }


  }
}
