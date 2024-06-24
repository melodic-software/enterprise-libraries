namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

public interface IHandler<in TRequest> :
    ICouldHandle<TRequest>, IShortCircuit, IHandle<TRequest>
    where TRequest : class;
