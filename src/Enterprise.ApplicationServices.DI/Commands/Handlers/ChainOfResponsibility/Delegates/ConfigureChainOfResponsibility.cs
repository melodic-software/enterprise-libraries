using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestOnly;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureChainOfResponsibility<TCommand>(ResponsibilityChainRegistrationBuilder<TCommand> builder)
    where TCommand : class, ICommand;
