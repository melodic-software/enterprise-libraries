using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;

public delegate IHandler<TCommand> HandlerImplementationFactory<in TCommand>(IServiceProvider provider);
