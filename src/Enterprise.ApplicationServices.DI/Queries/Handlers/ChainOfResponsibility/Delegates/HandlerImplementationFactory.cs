using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate IHandler<TQuery, TResult>
    HandlerImplementationFactory<in TQuery, TResult>(IServiceProvider provider);
