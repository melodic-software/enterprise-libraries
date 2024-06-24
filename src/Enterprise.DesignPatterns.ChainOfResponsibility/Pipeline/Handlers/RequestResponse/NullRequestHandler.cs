using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse;

public class NullRequestHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    public async Task<TResponse?> HandleAsync(TRequest request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        TResponse response = await next();
        return response;
    }
}
