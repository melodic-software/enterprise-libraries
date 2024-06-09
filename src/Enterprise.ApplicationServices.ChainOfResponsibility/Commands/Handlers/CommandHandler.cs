using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public sealed class CommandHandler<TCommand> : IHandleCommand<TCommand> where TCommand : class, ICommand
{
    private readonly IResponsibilityChain<TCommand> _responsibilityChain;

    public CommandHandler(IResponsibilityChain<TCommand> responsibilityChain)
    {
        _responsibilityChain = responsibilityChain;
    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await _responsibilityChain.HandleAsync(command, cancellationToken);
    }
}
