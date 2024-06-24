using Enterprise.FluentValidation.Services;
using FluentValidation;
using MediatR;

namespace Enterprise.MediatR.Behaviors.Validation;

public class RequestFluentValidationBehavior<TRequest, TResult>
    : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
{
    private readonly IReadOnlyCollection<IValidator<TRequest>> _validators;

    public RequestFluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators.ToList();
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        IValidationContext validationContext = new ValidationContext<TRequest>(request);

        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);

        return await next();
    }
}
