using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared;
using Microsoft.Extensions.Logging;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Chains;

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
        using IDisposable? requestScope = _logger.BeginScope("{RequestType}", typeof(TRequest).Name);

        foreach (IHandler<TRequest> handler in _handlers)
        {
            using IDisposable? handlerScope = _logger.BeginScope("{HandlerType}", handler.GetType().Name);

            // Each handler in the chain inspects the request to see if it can handle it.

            if (!handler.CanHandle(request))
            {
                _logger.LogInformation("The handler cannot handle the request. Continuing to the next handler.");
                continue;
            }

            handler.Handle(request);

            _logger.LogInformation("Request handled successfully.");

            // With the classic pattern, the chain is typically
            // short-circuited after the request is initially handled by a handler in the chain.
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

public class ResponsibilityChain<TRequest, TResponse> : IResponsibilityChain<TRequest, TResponse> where TRequest : class
{
    private readonly ILogger<ResponsibilityChain<TRequest, TResponse>> _logger;
    private readonly IReadOnlyCollection<IHandler<TRequest, TResponse?>> _handlers;

    public ResponsibilityChain(IEnumerable<IHandler<TRequest, TResponse?>> handlers, ILogger<ResponsibilityChain<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _handlers = handlers.ToList();
    }

    public TResponse? Handle(TRequest request)
    {
        using IDisposable? requestScope = _logger.BeginScope("{RequestType}, {ResponseType}", typeof(TRequest).Name, typeof(TResponse).Name);

        TResponse? response = default;

        foreach (IHandler<TRequest, TResponse?> handler in _handlers)
        {
            using IDisposable? handlerScope = _logger.BeginScope("{HandlerType}", handler.GetType().Name);

            // Each handler in the chain inspects the request to see if it can handle it.

            if (!handler.CanHandle(request))
            {
                _logger.LogInformation("The handler cannot handle the request. Continuing to the next handler.");
                continue;
            }

            response = handler.Handle(request);

            _logger.LogInformation("Request handled successfully.");

            // With the classic pattern, the chain is typically
            // short-circuited after the request is initially handled by a handler in the chain.
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

        return response;
    }
}
