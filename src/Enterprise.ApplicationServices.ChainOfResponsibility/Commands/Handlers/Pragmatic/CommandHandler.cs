using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public sealed class CommandHandler<TCommand, TResult> : IHandleCommand<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    private readonly IResponsibilityChain<TCommand, TResult> _responsibilityChain;

    public CommandHandler(IResponsibilityChain<TCommand, TResult> responsibilityChain)
    {
        _responsibilityChain = responsibilityChain;
    }

    /// <inheritdoc />
    async Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        return await _responsibilityChain.HandleAsync(command, cancellationToken);
    }
}
