namespace Enterprise.ApplicationServices.Core.Commands.Dispatching;

public interface IDispatchCommands
{
    Task DispatchAsync<TCommand>(TCommand command) where TCommand : IBaseCommand;
}
