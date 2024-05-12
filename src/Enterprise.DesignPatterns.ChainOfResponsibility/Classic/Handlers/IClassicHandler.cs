using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

public interface IClassicHandler<TRequest> :
    ISetClassicSuccessor<TRequest>, IHandle<TRequest>
    where TRequest : class;

public interface IClassicHandler<TRequest, TResponse> :
    ISetClassicSuccessor<TRequest, TResponse>, IHandle<TRequest, TResponse>
    where TRequest : class;
