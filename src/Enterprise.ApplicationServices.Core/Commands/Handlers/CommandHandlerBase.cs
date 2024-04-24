using Enterprise.ApplicationServices.Core.Standard;
using Enterprise.Events.Facade.Abstract;
using static Enterprise.ApplicationServices.Core.Commands.Handlers.CommandHandlerTypeValidationService;

namespace Enterprise.ApplicationServices.Core.Commands.Handlers;

public abstract class CommandHandlerBase<T> : ApplicationServiceBase, IHandleCommand<T> where T : IBaseCommand
{
    protected CommandHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {

    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command)
    {
        ValidateType(command, this);
        T typedCommand = (T)command;
        await HandleAsync(typedCommand);
    }

    /// <inheritdoc />
    public abstract Task HandleAsync(T command);
}

public abstract class CommandHandlerBase<TCommand, TResponse>
    : ApplicationServiceBase, IHandleCommand<TCommand>, IHandleCommand<TCommand, TResponse>
    where TCommand : IBaseCommand
{
    protected CommandHandlerBase(IEventServiceFacade eventServiceFacade) : base(eventServiceFacade)
    {

    }

    /// <inheritdoc />
    Task IHandleCommand<TCommand>.HandleAsync(TCommand command)
    {
        return HandleAsync(command);
    }

    /// <inheritdoc />
    public async Task HandleAsync(IBaseCommand command)
    {
        ValidateType(command, this);
        TCommand typedCommand = (TCommand)command;
        await HandleAsync(typedCommand);
    }

    /// <inheritdoc />
    public abstract Task<TResponse> HandleAsync(TCommand command);
}