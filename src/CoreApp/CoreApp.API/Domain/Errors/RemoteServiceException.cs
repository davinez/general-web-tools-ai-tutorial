using System;

namespace CoreApp.API.Domain.Errors;

public class RemoteServiceException : Exception
{
  public RemoteServiceException(string remoteServceName, string exceptionMessage)
        : base($"Remote Service \"{remoteServceName}\" failed with message {exceptionMessage}")
  {
  }
}
