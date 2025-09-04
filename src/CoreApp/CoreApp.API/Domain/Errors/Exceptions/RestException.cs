using System;
using System.Net;

namespace CoreApp.API.Domain.Errors.Exceptions;

public class RestException(HttpStatusCode code, object? errors = null) : Exception
{
    public object? Errors { get; } = errors;

    public HttpStatusCode Code { get; } = code;
}
