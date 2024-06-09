using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Pragmatic.Delegates;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Pragmatic;

public sealed class RegistrationOptions<TCommand, TResponse> :
    RegistrationOptionsBase<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Register decorators for the command handler.
    /// This is enabled by default.
    /// </summary>
    public bool UseDecorators { get; set; } = true;

    /// <summary>
    /// Provide custom delegates used for decorator composition.
    /// If none are provided, the default decorators will be used.
    /// <see cref="UseDecorators"/> must be true, otherwise decorator registrations will be skipped.
    /// </summary>
    public IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand, TResponse>> DecoratorFactories { get; } = [];

    /// <summary>
    /// A factory method delegate that instantiates the command handler instance.
    /// </summary>
    internal CommandHandlerImplementationFactory<TCommand, TResponse>? CommandHandlerImplementationFactory { get; }

    public RegistrationOptions(CommandHandlerImplementationFactory<TCommand, TResponse>? commandHandlerImplementationFactory)
    {
        CommandHandlerImplementationFactory = commandHandlerImplementationFactory;
    }
}
