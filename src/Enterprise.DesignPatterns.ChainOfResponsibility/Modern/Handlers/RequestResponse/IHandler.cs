namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestResponse;

public interface IHandler<in TRequest, out TResponse> :
    ICouldHandle<TRequest>, IShortCircuit, IHandle<TRequest, TResponse>
    where TRequest : class;
