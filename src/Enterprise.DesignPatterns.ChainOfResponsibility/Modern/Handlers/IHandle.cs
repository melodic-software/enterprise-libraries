namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

public interface IHandle<in TRequest> : IHandlerBase where TRequest : class
{
    void Handle(TRequest request);
}

public interface IHandle<in TRequest, out TResponse> : IHandlerBase where TRequest : class
{
    TResponse? Handle(TRequest request);
}
