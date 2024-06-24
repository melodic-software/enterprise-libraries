using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestOnly;

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
    public async Task HandleAsync(TRequest request, CancellationToken cancellationToken = default)
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
