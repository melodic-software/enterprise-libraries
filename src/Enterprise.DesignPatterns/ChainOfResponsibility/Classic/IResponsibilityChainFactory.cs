namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic;

public interface IResponsibilityChainFactory<TRequest> where TRequest : class
{
    IResponsibilityChain<TRequest> Create();
}

public interface IResponsibilityChainFactory<TRequest, TResponse> where TRequest : class
{
    IResponsibilityChain<TRequest, TResponse?> Create();
}
