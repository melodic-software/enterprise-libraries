using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;
using Enterprise.ApplicationServices.DI.Commands.Handlers.Options;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility;

public class RegistrationOptions<TCommand> : RegistrationOptionsBase<TCommand>
    where TCommand : IBaseCommand
{
    /// <summary>
    /// A factory method delegate that instantiates the chain of responsibility instance.
    /// </summary>
    internal HandlerImplementationFactory<TCommand>? CommandHandlerImplementationFactory { get; }

    /// <summary>
    /// Provide a custom responsibility chain configuration.
    /// If not provided, the default chain will be used.
    /// </summary>
    public Action<ResponsibilityChainRegistrationBuilder<TCommand>>? ConfigureChainOfResponsibility { get; set; }

    public RegistrationOptions(HandlerImplementationFactory<TCommand>? commandHandlerImplementationFactory)
    {
        CommandHandlerImplementationFactory = commandHandlerImplementationFactory;
    }
}
