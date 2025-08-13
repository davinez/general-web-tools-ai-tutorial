using Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.API.Infrastructure.Data;

/// <summary>
/// Adds transaction to the processing pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class DBContextTransactionPipelineBehavior<TRequest, TResponse>(CoreAppContext context)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
  public async ValueTask<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      MessageHandlerDelegate<TRequest, TResponse> next
  )
  {
    TResponse? result;

    try
    {
      context.BeginTransaction();

      result = await next(request, cancellationToken);

      context.CommitTransaction();
    }
    catch (Exception)
    {
      context.RollbackTransaction();
      throw;
    }

    return result;
  }
}
