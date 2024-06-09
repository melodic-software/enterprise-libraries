using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

/// <summary>
/// A base implementation of a command handler that supports the raising of events.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public abstract class CommandHandlerBase<TCommand> : 
    ApplicationServiceBase, IHandleCommand<TCommand>, IHandler<TCommand>
    where TCommand : class, IBaseCommand
{
    protected CommandHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {

    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken)
    {
        await HandleAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
