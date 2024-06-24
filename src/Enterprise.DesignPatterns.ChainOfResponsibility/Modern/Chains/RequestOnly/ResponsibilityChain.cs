using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestOnly;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains.RequestOnly;

public class ResponsibilityChain<TRequest> : IResponsibilityChain<TRequest> where TRequest : class
{
    private readonly ILogger<ResponsibilityChain<TRequest>> _logger;
    private readonly IReadOnlyCollection<IHandler<TRequest>> _handlers;

    public ResponsibilityChain(IEnumerable<IHandler<TRequest>> handlers, ILogger<ResponsibilityChain<TRequest>> logger)
    {
        _logger = logger;
        _handlers = handlers.ToList();
    }

    public void Handle(TRequest request)
    {
        using IDisposable? requestScope = _logger.BeginScope("{@Request}", request);

        foreach (IHandler<TRequest> handler in _handlers)
        {
            using IDisposable? handlerScope = _logger.BeginScope("{@Handler}", handler);

            // Each handler in the chain inspects the request to see if it can handle it.

            if (!handler.CanHandle(request))
            {
                _logger.LogInformation("The handler cannot handle the request. Continuing to the next handler.");
                continue;
            }

            handler.Handle(request);

            _logger.LogInformation("Request handled successfully.");

            // With the classic pattern, the chain is typically short-circuited after the request is initially handled by a handler in the chain.
            // We leave it up to the handler to determine if the request should still be forwarded onto other handlers or not.

            if (handler.ShortCircuit)
            {
                _logger.LogInformation(
                    "Short circuiting the chain of responsibility. " +
                    "No more handlers will receive the request."
                );

                break;
            }
        }
    }
}
