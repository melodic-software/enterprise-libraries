using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Pragmatic;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Library.Core.Attributes;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Pragmatic;

/// <summary>
/// A pragmatic base implementation of a command handler that returns a result.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
[AlternativeTo(typeof(CommandHandlerBase<>))]
public abstract class CommandHandlerBase<TCommand, TResult>
    : ApplicationServiceBase, IHandleCommand<TCommand, TResult>, IHandler<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
{
    protected CommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    /// <inheritdoc />
    public async Task HandleAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    async Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult?> HandleAsync(TCommand request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        return await HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
