namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestResponse;

public interface ISetSuccessor<TRequest, TResponse> : IHandlerBase where TRequest : class
{
    IHandler<TRequest, TResponse?> SetSuccessor(IHandler<TRequest, TResponse?> successor);
}
