using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic.Delegates;

public delegate IHandleCommand<TCommand, TResult>
    CommandHandlerDecoratorImplementationFactory<TCommand, TResult>(IServiceProvider provider, IHandleCommand<TCommand, TResult> commandHandler)
    where TCommand : class, ICommand<TResult>;
