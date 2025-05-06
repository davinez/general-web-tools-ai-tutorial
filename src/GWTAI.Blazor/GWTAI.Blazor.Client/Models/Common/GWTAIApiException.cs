using System.Net;

namespace GWTAI.Blazor.Client.Models.Common;

public class GWTAIApiException : Exception
{
  public GWTAIApiException(string message, HttpStatusCode statusCode, IEnumerable<string>? apiErrors = null)
           : base(message)
  {
    StatusCode = statusCode;
    ApiErrors = apiErrors;
  }

  public HttpStatusCode StatusCode { get; }

  public IEnumerable<string>? ApiErrors { get; }

}
