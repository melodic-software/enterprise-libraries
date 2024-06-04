using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureChainOfResponsibility<TCommand>(ResponsibilityChainRegistrationBuilder<TCommand> builder)
    where TCommand : IBaseCommand;
