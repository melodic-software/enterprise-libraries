using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;
using Enterprise.DesignPatterns.ChainOfResponsibility.Shared;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Chains;

public class ClassicResponsibilityChain<TRequest> : IResponsibilityChain<TRequest> where TRequest : class
{
    private readonly IReadOnlyCollection<IClassicHandler<TRequest>> _handlers;

    public ClassicResponsibilityChain(IEnumerable<IClassicHandler<TRequest>> handlers)
    {
        _handlers = handlers.ToList();
    }

    public void Handle(TRequest request)
    {
        foreach (IClassicHandler<TRequest> handler in _handlers)
        {
            handler.Handle(request);
        }
    }
}

public class ClassicResponsibilityChain<TRequest, TResponse> : IResponsibilityChain<TRequest, TResponse> where TRequest : class
{
    private readonly IReadOnlyCollection<IClassicHandler<TRequest, TResponse?>> _handlers;

    public ClassicResponsibilityChain(IEnumerable<IClassicHandler<TRequest, TResponse?>> handlers)
    {
        _handlers = handlers.ToList();
    }

    public TResponse? Handle(TRequest request)
    {
        TResponse? response = default;

        foreach (IClassicHandler<TRequest, TResponse?> handler in _handlers)
        {
            response = handler.Handle(request);
        }

        return response;
    }
}
