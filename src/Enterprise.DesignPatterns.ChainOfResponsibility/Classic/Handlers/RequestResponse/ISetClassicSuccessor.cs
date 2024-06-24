namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestResponse;

public interface ISetClassicSuccessor<TRequest, TResponse> where TRequest : class
{
    IClassicHandler<TRequest, TResponse?> SetSuccessor(IClassicHandler<TRequest, TResponse?> successor);
}
