using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic;

public class ResponsibilityChain<TRequest> : IResponsibilityChain<TRequest> where TRequest : class
{
    private List<IHandler<TRequest>> Handlers { get; } = [];

    public void SetNext(IHandler<TRequest> next) => Add(next);

    public void Add(IHandler<TRequest> handler)
    {
        Handlers.Add(handler);
    }

    public void Handle(TRequest request)
    {
        foreach (IHandler<TRequest> handler in Handlers)
        {
            handler.Handle(request);
        }
    }
}

public class ResponsibilityChain<TRequest, TResponse> : IResponsibilityChain<TRequest, TResponse> where TRequest : class
{
    private List<IHandler<TRequest, TResponse?>> Handlers { get; } = [];

    protected virtual TResponse? DefaultResponse => default;
    protected Type? CurrentHandlerType { get; private set; }

    public void SetNext(IHandler<TRequest, TResponse?> next) => Add(next);

    public void Add(IHandler<TRequest, TResponse?> handler)
    {
        Handlers.Add(handler);
    }

    public TResponse? Handle(TRequest request)
    {
        TResponse? response = DefaultResponse;

        foreach (IHandler<TRequest, TResponse?> handler in Handlers)
        {
            CurrentHandlerType = handler.GetType();

            PreHandle(request);

            if (QuitProcessing(request))
                break;

            response = handler.Handle(request);

            PostHandle(request);
        }

        return response;
    }

    /// <summary>
    /// Override to add some additional code before calling the next link in the chain.
    /// Return true to return early and short circuit the chain of execution.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected virtual bool PreHandle(TRequest request)
    {
        return false;
    }

    protected virtual bool QuitProcessing(TRequest request)
    {
        return false;
    }

    /// <summary>
    /// Override to add some additional code after the request has been handled by the current link in the chain.
    /// </summary>
    /// <param name="request"></param>
    protected virtual void PostHandle(TRequest request)
    {

    }
}
