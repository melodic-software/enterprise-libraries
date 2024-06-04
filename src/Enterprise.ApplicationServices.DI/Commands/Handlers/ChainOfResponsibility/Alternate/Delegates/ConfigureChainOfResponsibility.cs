using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Alternate.Delegates;

public delegate void ConfigureChainOfResponsibility<TCommand, TResponse>(ResponsibilityChainRegistrationBuilder<TCommand, TResponse> builder)
    where TCommand : IBaseCommand;
