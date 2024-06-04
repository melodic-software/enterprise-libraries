using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Alternate.Delegates;

public delegate IHandleCommand<TCommand, TResponse>
    CommandHandlerDecoratorImplementationFactory<TCommand, TResponse>(IServiceProvider provider, IHandleCommand<TCommand, TResponse> commandHandler)
    where TCommand : ICommand<TResponse>;
