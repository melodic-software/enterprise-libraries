using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public class NullCommandValidationCommandHandler<TCommand> : IHandler<TCommand>
{
    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        await next();
    }
}
