using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline;

/// <inheritdoc />
public sealed class ResponsibilityChain<TRequest> : IResponsibilityChain<TRequest>
{
    private readonly IEnumerable<IChainLink<TRequest>> _handlers;

    public ResponsibilityChain(IEnumerable<IChainLink<TRequest>> handlers)
    {
        _handlers = handlers.ToList();
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        NextChainLinkDelegate requestDelegate = () => Task.CompletedTask;

        // Chain asynchronous handlers in reverse order.
        foreach (IChainLink<TRequest> handler in _handlers.Reverse())
        {
            NextChainLinkDelegate current = requestDelegate;
            requestDelegate = () => handler.ExecuteAsync(request, current, cancellationToken);
        }

        // Execute the chain.
        await requestDelegate();
    }
}

/// <inheritdoc />
public sealed class ResponsibilityChain<TRequest, TResponse> : IResponsibilityChain<TRequest, TResponse>
{
    private readonly IEnumerable<IChainLink<TRequest, TResponse?>> _handlers;

    public ResponsibilityChain(IEnumerable<IChainLink<TRequest, TResponse?>> handlers)
    {
        _handlers = handlers.ToList();
    }

    /// <inheritdoc />
    public async Task<TResponse?> ExecuteAsync(TRequest request, CancellationToken cancellationToken)
    {
        // Start with a delegate that returns a default response.
        NextChainLinkDelegate<TResponse?> handlerDelegate = () => Task.FromResult<TResponse?>(default);

        // Chain asynchronous handlers in reverse order.
        foreach (IChainLink<TRequest, TResponse?> handler in _handlers.Reverse())
        {
            NextChainLinkDelegate<TResponse?> next = handlerDelegate;
            handlerDelegate = () => handler.ExecuteAsync(request, next, cancellationToken);
        }

        // Execute the chain.
        return await handlerDelegate();
    }
}
