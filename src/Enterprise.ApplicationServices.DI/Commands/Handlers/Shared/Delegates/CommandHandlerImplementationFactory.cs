using Enterprise.ApplicationServices.Core.Commands.Handlers.Strict;
using Enterprise.ApplicationServices.Core.Commands.Model.Strict;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates;

public delegate CommandHandlerBase<TCommand>
    CommandHandlerImplementationFactory<TCommand>(IServiceProvider provider)
    where TCommand : class, ICommand;
