namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestResponse;

public interface IHandle<in TRequest, out TResponse> : IHandlerBase where TRequest : class
{
    TResponse? Handle(TRequest request);
}
