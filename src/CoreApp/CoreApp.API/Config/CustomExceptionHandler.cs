using CoreApp.API.Domain.Errors;
using CoreApp.API.Domain.Errors.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Config;

public class CustomExceptionHandler : IExceptionHandler
{
  private readonly ILogger<CustomExceptionHandler> _logger;
  private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

  public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
  {
    _logger = logger;
    // Register known exception types and handlers.
    _exceptionHandlers = new()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(RemoteServiceException), HandleRemoteServiceException },
                { typeof(CoreAppException), HandleCoreAppException },
            };
  }

  // ASP.NET Core log an unhandled exception even if a global exception handler is used
  // https://learn.microsoft.com/en-us/answers/questions/1618819/why-does-asp-net-core-log-an-unhandled-exception-w
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    var exceptionType = exception.GetType();

    if (_exceptionHandlers.ContainsKey(exceptionType))
    {
      await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
      return true;
    }

    // Unhandled / Not specified exception
    await UnhandledException(httpContext, exception);

    return await ValueTask.FromResult(true);
  }

  private async Task HandleValidationException(HttpContext httpContext, Exception ex)
  {
    var exception = (ValidationException)ex;

    string detailedException = $"Source: {exception.Source} Inner Exception: {exception.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: ValidationException",
           exception.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    var errors = exception.Errors.Select(e => new ApiError()
    {
      Domain = e.Key,
      Message = string.Join("|", e.Value)
    }).ToList();

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status400BadRequest,
        Message = "Validation Problem. https://tools.ietf.org/html/rfc7231#section-6.5.1",
        Errors = errors
      }

    });
  }

  private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
  {
    var exception = (NotFoundException)ex;

    string detailedException = $"Source: {exception.Source} Inner Exception: {exception.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: NotFoundException",
           exception.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status404NotFound,
        Message = "The specified resource was not found.",
        Errors =
            [
                new()
                    {
                        Domain = exception.Source,
                        Reason = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        Message = exception.Message
                    }
            ]
      }

    });
  }

  private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
  {
    string detailedException = $"Source: {ex.Source} Inner Exception: {ex.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: UnauthorizedAccessException",
           ex.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status401Unauthorized,
        Message = "Unauthorized",
        Errors =
           [
               new()
                    {
                        Domain = "",
                        Reason = "https://tools.ietf.org/html/rfc7235#section-3.1",
                        Message = ""
                    }
           ]
      }

    });
  }

  private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
  {
    string detailedException = $"Source: {ex.Source} Inner Exception: {ex.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: ForbiddenAccessException",
           ex.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status403Forbidden,
        Message = "Forbidden",
        Errors =
            [
                new()
                    {
                        Domain = "",
                        Reason = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                        Message = ""
                    }
            ]
      }

    });
  }

  private async Task HandleRemoteServiceException(HttpContext httpContext, Exception ex)
  {
    string detailedException = $"Source: {ex.Source} Inner Exception: {ex.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: RemoteServiceException",
           ex.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status409Conflict,
        Message = "Conflict",
        Errors =
            [
                new()
                    {
                        Domain = "",
                        Reason = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                        Message = ""
                    }
            ]
      }

    });
  }

  private async Task HandleCoreAppException(HttpContext httpContext, Exception ex)
  {
    var exception = (CoreAppException)ex;

    string detailedException = $"Source: {exception.Source} Inner Exception: {exception.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: ManagerException",
           exception.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status409Conflict,
        Message = "Conflict",
        Errors =
            [
                new()
                    {
                        Domain = exception.Source,
                        Reason = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                        Message = exception.Message,
                    }
            ]
      }

    });
  }

  private async Task UnhandledException(HttpContext httpContext, Exception ex)
  {
    string detailedException = $"Source: {ex.Source} Inner Exception: {ex.InnerException?.Message}";

    _logger.LogError(
           "Error Message: {Message}, Detailed Message: {DetailedException} Type: UnhandledException",
           ex.Message, detailedException);

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

    await httpContext.Response.WriteAsJsonAsync(new ApiErrorResponse()
    {
      ApiVersion = "1.0",
      Error = new ApiTopLevelError()
      {
        Code = StatusCodes.Status500InternalServerError,
        Message = "UnhandledException",
        Errors =
            [
                new()
                    {
                        Domain = ex.Source,
                        Reason = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                        Message = "Unexpected error occurred",
                    }
            ]
      }

    });
  }
}

