using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;

namespace Enterprise.ApplicationServices.DI.Commands.Handlers.ChainOfResponsibility.Pragmatic.Delegates;

public delegate void ConfigureChainOfResponsibility<TCommand, TResult>(ResponsibilityChainRegistrationBuilder<TCommand, TResult> builder)
    where TCommand : class, ICommand;
