namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

public interface IHandle<in TRequest> : IHandlerBase where TRequest : class
{
    void Handle(TRequest request);
}
