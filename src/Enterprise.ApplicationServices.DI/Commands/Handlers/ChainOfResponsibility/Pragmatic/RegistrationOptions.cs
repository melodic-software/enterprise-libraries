using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options.Pragmatic;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic;

public class RegistrationOptions<TCommand, TResult> : RegistrationOptionsBase<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// If not provided, the default chain will be used.
    /// </summary>
    public ConfigureChainOfResponsibility<TCommand, TResult>? ConfigureChainOfResponsibility { get; set; }

    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility instance.
    /// </summary>
    internal HandlerImplementationFactory<TCommand, TResult>? CommandHandlerImplementationFactory { get; }

    public RegistrationOptions(HandlerImplementationFactory<TCommand, TResult>? commandHandlerImplementationFactory)
    {
        CommandHandlerImplementationFactory = commandHandlerImplementationFactory;
    }
}
