using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Delegates;

public delegate IHandleCommand<TCommand>
    CommandHandlerDecoratorImplementationFactory<TCommand>(IServiceProvider provider, IHandleCommand<TCommand> commandHandler)
    where TCommand : class, ICommand;
