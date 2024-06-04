using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Delegates;

public delegate IHandleCommand<TCommand>
    CommandHandlerDecoratorImplementationFactory<TCommand>(IServiceProvider provider, IHandleCommand<TCommand> commandHandler)
    where TCommand : IBaseCommand;
