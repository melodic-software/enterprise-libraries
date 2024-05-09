using Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic;

public interface IResponsibilityChain<TRequest> : IHandler<TRequest> where TRequest : class
{
    void Add(IHandler<TRequest> handler);
}

public interface IResponsibilityChain<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    void Add(IHandler<TRequest, TResponse?> handler);
}
