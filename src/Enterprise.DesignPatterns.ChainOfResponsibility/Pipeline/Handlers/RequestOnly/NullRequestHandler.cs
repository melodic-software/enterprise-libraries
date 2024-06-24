using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly;

public class NullRequestHandler<TRequest> : IHandler<TRequest>
{
    public async Task HandleAsync(TRequest request, SuccessorDelegate next, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        await next();
    }
}
