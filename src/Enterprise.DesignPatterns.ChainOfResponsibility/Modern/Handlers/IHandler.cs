namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

public interface IHandler<in TRequest> :
    ICouldHandle<TRequest>, IShortCircuit, IHandle<TRequest>
    where TRequest : class;

public interface IHandler<in TRequest, out TResponse> :
    ICouldHandle<TRequest>, IShortCircuit, IHandle<TRequest, TResponse>
    where TRequest : class;
