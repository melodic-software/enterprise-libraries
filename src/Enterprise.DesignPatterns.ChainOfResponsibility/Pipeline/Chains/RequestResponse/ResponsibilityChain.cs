using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;

/// <inheritdoc />
public sealed class ResponsibilityChain<TRequest, TResponse> : IResponsibilityChain<TRequest, TResponse>
{
    private readonly IEnumerable<IHandler<TRequest, TResponse?>> _handlers;

    /// <summary>
    /// Initializes a new instance of the ResponsibilityChain class.
    /// </summary>
    /// <param name="handlers">The collection of handlers that will process the requests and produce a response.</param>
    public ResponsibilityChain(IEnumerable<IHandler<TRequest, TResponse?>> handlers)
    {
        _handlers = handlers.ToList();
    }

    /// <inheritdoc />
    public async Task<TResponse?> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        // Start with a delegate that returns a default response.
        SuccessorDelegate<TResponse?> handlerDelegate = () => Task.FromResult<TResponse?>(default);

        // Chain asynchronous handlers in reverse order.
        foreach (IHandler<TRequest, TResponse?> handler in _handlers.Reverse())
        {
            SuccessorDelegate<TResponse?> next = handlerDelegate;
            handlerDelegate = () => handler.HandleAsync(request, next, cancellationToken);
        }

        // Execute the chain.
        return await handlerDelegate();
    }
}
