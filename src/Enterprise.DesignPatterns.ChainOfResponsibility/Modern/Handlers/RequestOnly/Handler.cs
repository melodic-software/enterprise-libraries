namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

public abstract class Handler<TRequest> : IHandler<TRequest> where TRequest : class
{
    /// <summary>
    /// Determines if the chain of responsibility should be short-circuited.
    /// With the classic form, this is typically done after the first handler in the chain is able to handle the request.
    /// This is defaulted to true, but can be overriden to allow the next handler in the chain to process the request in this scenario.
    /// </summary>
    public virtual bool ShortCircuit => true;

    /// <summary>
    /// Each handler in the chain inspects the request to see if it can be handled.
    /// If it can't, the request is passed on to the next handler in the chain.
    /// By default, all handlers can handle the type specified.
    /// This can be overriden if specific logic needs to be added.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public virtual bool CanHandle(TRequest request) => true;

    public abstract void Handle(TRequest request);
}
