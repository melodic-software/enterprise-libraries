using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;

/// <inheritdoc />
public sealed class ResponsibilityChain<TRequest> : IResponsibilityChain<TRequest>
{
    private readonly IEnumerable<IHandler<TRequest>> _handlers;

    /// <summary>
    /// Initializes a new instance of the ResponsibilityChain class.
    /// </summary>
    /// <param name="handlers">The collection of handlers that will process the requests.</param>
    public ResponsibilityChain(IEnumerable<IHandler<TRequest>> handlers)
    {
        _handlers = handlers.ToList();
    }

    /// <inheritdoc />
    public async Task HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        SuccessorDelegate requestDelegate = () => Task.CompletedTask;

        // Chain asynchronous handlers in reverse order.
        foreach (IHandler<TRequest> handler in _handlers.Reverse())
        {
            SuccessorDelegate current = requestDelegate;
            requestDelegate = () => handler.HandleAsync(request, current, cancellationToken);
        }

        // Execute the chain.
        await requestDelegate();
    }
}

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
    public async Task<TResponse?> HandleAsync(TRequest request, CancellationToken cancellationToken)
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
