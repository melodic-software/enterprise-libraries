namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

public interface ISetSuccessor<TRequest> : IHandlerBase where TRequest : class
{
    IHandler<TRequest> SetSuccessor(IHandler<TRequest> successor);
}
