namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

public interface ISetClassicSuccessor<TRequest> where TRequest : class
{
    IClassicHandler<TRequest> SetSuccessor(IClassicHandler<TRequest> successor);
}

public interface ISetClassicSuccessor<TRequest, TResponse> where TRequest : class
{
    IClassicHandler<TRequest, TResponse?> SetSuccessor(IClassicHandler<TRequest, TResponse?> successor);
}
