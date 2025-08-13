using FluentValidation;
using Mediator;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure;

public class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
  private readonly List<IValidator<TRequest>> _validators = validators.ToList();

  public async ValueTask<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      MessageHandlerDelegate<TRequest, TResponse> next    
  )
  {
    TResponse? result;

    var context = new ValidationContext<TRequest>(request);
    var failures = _validators
        .Select(v => v.Validate(context))
        .SelectMany(result => result.Errors)
        .Where(f => f != null)
        .ToList();

    if (failures.Count != 0)
    {
      throw new ValidationException(failures);
    }

    result = await next(request, cancellationToken);

    return result;
  }
}
