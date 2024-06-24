using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;

public delegate IHandler<TCommand, TResult> HandlerImplementationFactory<in TCommand, TResult>(IServiceProvider provider);
