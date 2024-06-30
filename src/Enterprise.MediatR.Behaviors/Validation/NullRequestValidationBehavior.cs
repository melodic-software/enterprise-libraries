using MediatR;

namespace Enterprise.MediatR.Behaviors.Validation;

public class NullRequestValidationBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult> where TRequest : class
{
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        TResult result = await next();
        return result;
    }
}
