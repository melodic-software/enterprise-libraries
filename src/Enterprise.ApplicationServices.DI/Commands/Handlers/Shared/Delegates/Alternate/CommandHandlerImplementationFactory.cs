using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Alternate;

public delegate CommandHandlerBase<TCommand, TResponse>
    CommandHandlerImplementationFactory<TCommand, TResponse>(IServiceProvider provider)
    where TCommand : ICommand<TResponse>;

public delegate IHandleCommand<TCommand>
    CommandHandlerImplementationFactory<in TCommand>(IServiceProvider provider)
    where TCommand : IBaseCommand;
