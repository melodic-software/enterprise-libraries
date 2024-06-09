using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Pragmatic;

public delegate CommandHandlerBase<TCommand, TResponse>
    CommandHandlerImplementationFactory<TCommand, TResponse>(IServiceProvider provider)
    where TCommand : ICommand<TResponse>;
