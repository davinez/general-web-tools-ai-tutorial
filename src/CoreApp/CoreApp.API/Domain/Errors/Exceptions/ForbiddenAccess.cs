using System;

namespace CoreApp.API.Domain.Errors.Exceptions;

public class ForbiddenAccessException : Exception
{
  public ForbiddenAccessException() : base() { }
}

