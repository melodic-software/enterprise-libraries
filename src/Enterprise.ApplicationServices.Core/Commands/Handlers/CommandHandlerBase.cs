using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

public abstract class CommandHandlerBase<TCommand> : 
    ApplicationServiceBase, IHandleCommand<TCommand>
    where TCommand : IBaseCommand
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
    public abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
