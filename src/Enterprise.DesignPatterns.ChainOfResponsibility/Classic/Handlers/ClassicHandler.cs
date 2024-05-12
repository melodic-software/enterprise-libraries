namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

public abstract class ClassicHandler<TRequest> : IClassicHandler<TRequest> where TRequest : class
{
    protected IClassicHandler<TRequest>? Successor { get; private set; }

    public IClassicHandler<TRequest> SetSuccessor(IClassicHandler<TRequest> successor)
    {
        Successor = successor;

        // Returning a handler from here allows for a fluent API,
        // making it easier to add multiple links in the chain.

        return successor;
    }

    public abstract void Handle(TRequest request);
}

public abstract class ClassicHandler<TRequest, TResponse> : IClassicHandler<TRequest, TResponse> where TRequest : class
{
    protected IClassicHandler<TRequest, TResponse?>? Successor { get; private set; }

    public IClassicHandler<TRequest, TResponse?> SetSuccessor(IClassicHandler<TRequest, TResponse?> successor)
    {
        Successor = successor;
        
        // Returning a handler from here allows for a fluent API,
        // making it easier to add multiple links in the chain.
        
        return successor;
    }

    public abstract TResponse? Handle(TRequest request);
}
