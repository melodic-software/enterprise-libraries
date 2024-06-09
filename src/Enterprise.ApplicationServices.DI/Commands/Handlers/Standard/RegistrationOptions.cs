using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Shared.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Standard.Decoration.Delegates;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard;

public sealed class RegistrationOptions<TCommand> :
    RegistrationOptionsBase<TCommand>
    where TCommand : class, ICommand
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
    public IEnumerable<CommandHandlerDecoratorImplementationFactory<TCommand>> DecoratorFactories { get; } = [];

    /// <summary>
    /// A factory method delegate that instantiates the command handler instance.
    /// </summary>
    internal CommandHandlerImplementationFactory<TCommand>? CommandHandlerImplementationFactory { get; }

    public RegistrationOptions(CommandHandlerImplementationFactory<TCommand>? commandHandlerImplementationFactory)
    {
        CommandHandlerImplementationFactory = commandHandlerImplementationFactory;
    }
}
