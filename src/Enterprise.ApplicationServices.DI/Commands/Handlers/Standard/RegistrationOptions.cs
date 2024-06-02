using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.Standard;

public sealed class RegistrationOptions<TCommand> :
    RegistrationOptionsBase<TCommand>
    where TCommand : IBaseCommand
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
    public IEnumerable<Func<IServiceProvider, IHandleCommand<TCommand>, IHandleCommand<TCommand>>> DecoratorFactories { get; } = [];

    /// <summary>
    /// A factory method delegate that instantiates the command handler instance.
    /// </summary>
    internal Func<IServiceProvider, IHandleCommand<TCommand>>? CommandHandlerImplementationFactory { get; }

    public RegistrationOptions(Func<IServiceProvider, IHandleCommand<TCommand>>? commandHandlerImplementationFactory)
    {
        CommandHandlerImplementationFactory = commandHandlerImplementationFactory;
    }
}
