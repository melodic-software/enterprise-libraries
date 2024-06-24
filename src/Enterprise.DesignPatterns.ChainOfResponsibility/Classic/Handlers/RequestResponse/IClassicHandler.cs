using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestResponse;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestResponse;

public interface IClassicHandler<TRequest, TResponse> :
    ISetClassicSuccessor<TRequest, TResponse>, IHandle<TRequest, TResponse>
    where TRequest : class;
