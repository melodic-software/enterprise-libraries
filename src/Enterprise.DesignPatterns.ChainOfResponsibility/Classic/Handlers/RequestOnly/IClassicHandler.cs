using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers.RequestOnly;

public interface IClassicHandler<TRequest> :
    ISetClassicSuccessor<TRequest>, IHandle<TRequest>
    where TRequest : class;
