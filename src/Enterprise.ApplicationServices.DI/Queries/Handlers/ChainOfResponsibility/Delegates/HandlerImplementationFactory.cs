using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate IHandler<TQuery, TResponse>
    HandlerImplementationFactory<in TQuery, TResponse>(IServiceProvider provider);
