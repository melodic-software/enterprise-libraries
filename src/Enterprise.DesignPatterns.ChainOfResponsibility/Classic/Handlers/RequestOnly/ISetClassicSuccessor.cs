namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestOnly;

public interface ISetClassicSuccessor<TRequest> where TRequest : class
{
    IClassicHandler<TRequest> SetSuccessor(IClassicHandler<TRequest> successor);
}
