using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic.Delegates;

public delegate IHandleCommand<TCommand, TResult>
    CommandHandlerDecoratorImplementationFactory<TCommand, TResult>(IServiceProvider provider, IHandleCommand<TCommand, TResult> commandHandler)
    where TCommand : ICommand<TResult>;
