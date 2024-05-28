using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Commands.Model.Alternate;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Events.Facade.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers.Alternate;

public abstract class CommandHandlerBase<TCommand, TResponse>
    : ApplicationServiceBase, IHandleCommand<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    protected CommandHandlerBase(IEventRaisingFacade eventService) : base(eventService)
    {
    }

    /// <inheritdoc />
    async Task IHandleCommand<TCommand>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
    }

    /// <inheritdoc />
    public async Task HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (TCommand)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
