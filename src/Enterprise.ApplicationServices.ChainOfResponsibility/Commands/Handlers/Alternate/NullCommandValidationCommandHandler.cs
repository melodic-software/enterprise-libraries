using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Commands.Handlers.Alternate;

public class NullCommandValidationCommandHandler<TCommand, TResponse> : IHandler<TCommand, TResponse>
{
    public async Task<TResponse?> HandleAsync(TCommand request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await next();
    }
}
