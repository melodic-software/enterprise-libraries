using Enterprise.ApplicationServices.Core.Commands.Model;
using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.Validation.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

public abstract class CommandHandlerBase<T> : ApplicationServiceBase, IHandleCommand<T> where T : ICommand
{
    protected CommandHandlerBase(IEventRaisingFacade eventRaisingFacade) : base(eventRaisingFacade)
    {

    }

    /// <inheritdoc />
    public async Task HandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        ValidateType(command, this);
        var typedCommand = (T)command;
        await HandleAsync(typedCommand, cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T command, CancellationToken cancellationToken);
}
