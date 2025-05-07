using System;

namespace CoreApp.API.Infrastructure.Errors;

public class CoreAppException : Exception
{
  public CoreAppException()
  { }

  public CoreAppException(string message)
      : base(message)
  { }

  public CoreAppException(string message, Exception innerException)
      : base(message, innerException)
  { }
}
