using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers;

public class NullCommandValidationCommandHandler<TCommand, TResponse> : IHandler<TCommand>, IHandler<TCommand, TResponse>
{
    public async Task HandleAsync(TCommand request, SuccessorDelegate next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        await next();
    }

    public Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
