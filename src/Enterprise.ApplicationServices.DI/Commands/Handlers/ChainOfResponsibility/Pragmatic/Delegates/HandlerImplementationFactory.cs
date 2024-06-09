using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;

public delegate IHandler<TCommand, TResult> HandlerImplementationFactory<in TCommand, TResult>(IServiceProvider provider);
