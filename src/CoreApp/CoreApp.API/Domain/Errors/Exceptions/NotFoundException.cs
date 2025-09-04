using System;

namespace CoreApp.API.Domain.Errors.Exceptions;


public class NotFoundException : Exception
{
  public NotFoundException()
  { }

  public NotFoundException(string message)
      : base(message)
  { }

  public NotFoundException(string message, Exception innerException)
      : base(message, innerException)
  { }

}


