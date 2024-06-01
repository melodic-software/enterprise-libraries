using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class NullQueryValidationQueryHandler<TQuery, TResponse> : IHandler<TQuery, TResponse>
{
    public async Task<TResponse?> HandleAsync(TQuery request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await next();
    }
}
