namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

public interface ISetSuccessor<TRequest> : IHandlerBase where TRequest : class
{
    IHandler<TRequest> SetSuccessor(IHandler<TRequest> successor);
}

public interface ISetSuccessor<TRequest, TResponse> : IHandlerBase where TRequest : class
{
    IHandler<TRequest, TResponse?> SetSuccessor(IHandler<TRequest, TResponse?> successor);
}
