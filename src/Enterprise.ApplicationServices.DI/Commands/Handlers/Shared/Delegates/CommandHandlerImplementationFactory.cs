using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates;

public delegate CommandHandlerBase<TCommand>
    CommandHandlerImplementationFactory<TCommand>(IServiceProvider provider)
    where TCommand : class, ICommand;
