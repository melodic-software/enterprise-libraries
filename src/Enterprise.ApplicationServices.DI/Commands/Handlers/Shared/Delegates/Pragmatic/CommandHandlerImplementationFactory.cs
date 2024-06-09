using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Pragmatic;

public delegate CommandHandlerBase<TCommand, TResult>
    CommandHandlerImplementationFactory<TCommand, TResult>(IServiceProvider provider)
    where TCommand : class, ICommand<TResult>;
