using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class NullQueryValidationQueryHandler<TQuery, TResult> : IHandler<TQuery, TResult>
{
    public async Task<TResult?> HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await next();
    }
}
