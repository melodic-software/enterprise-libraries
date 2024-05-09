using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic;

public class ResponsibilityChainFactory<TRequest> : IResponsibilityChainFactory<TRequest> where TRequest : class
{
    private readonly IEnumerable<IHandler<TRequest>> _handlers;

    public ResponsibilityChainFactory(IEnumerable<IHandler<TRequest>> handlers)
    {
        _handlers = handlers;
    }

    public IResponsibilityChain<TRequest> Create()
    {
        IResponsibilityChain<TRequest> chain = new ResponsibilityChain<TRequest>();

        foreach (IHandler<TRequest> handler in _handlers)
        {
            chain.Add(handler);
        }

        return chain;
    }
}

public class ResponsibilityChainFactory<TRequest, TResponse> : IResponsibilityChainFactory<TRequest, TResponse> where TRequest : class
{
    private readonly IEnumerable<IHandler<TRequest, TResponse?>> _handlers;

    public ResponsibilityChainFactory(IEnumerable<IHandler<TRequest, TResponse?>> handlers)
    {
        _handlers = handlers;
    }

    public IResponsibilityChain<TRequest, TResponse?> Create()
    {
        IResponsibilityChain<TRequest, TResponse?> chain = new ResponsibilityChain<TRequest, TResponse?>();

        foreach (IHandler<TRequest, TResponse?> handler in _handlers)
        {
            chain.Add(handler);
        }

        return chain;
    }
}
