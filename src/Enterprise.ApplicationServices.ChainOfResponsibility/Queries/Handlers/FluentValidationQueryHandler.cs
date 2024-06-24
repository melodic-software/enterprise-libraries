using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Enterprise.FluentValidation.Services;
using FluentValidation;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class FluentValidationQueryHandler<TQuery, TResult> : IHandler<TQuery, TResult>
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;

    public FluentValidationQueryHandler(IEnumerable<IValidator<TQuery>> validators)
    {
        _validators = validators.ToList();
    }

    public async Task<TResult?> HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        IValidationContext validationContext = new ValidationContext<TQuery>(request);

        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);

        return await next();
    }
}
