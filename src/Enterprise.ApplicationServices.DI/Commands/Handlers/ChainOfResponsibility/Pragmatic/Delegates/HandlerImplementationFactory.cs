using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;

public delegate IHandler<TCommand, TResponse> HandlerImplementationFactory<in TCommand, TResponse>(IServiceProvider provider);
