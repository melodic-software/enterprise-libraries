using Enterprise.ApplicationServices.Core.Commands.Handlers;
using Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;
using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Pragmatic;

public sealed class CommandHandler<TCommand, TResult> : IHandleCommand<TCommand, TResult> 
    where TCommand : class, ICommand<TResult>
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
    public async Task HandleAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        return await _responsibilityChain.HandleAsync(command, cancellationToken);
    }
}
