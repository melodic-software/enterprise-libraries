using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers;
using Enterprise.FluentValidation.Services;
using FluentValidation;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class FluentValidationQueryHandler<TQuery, TResponse> : IHandler<TQuery, TResponse>
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;

    public FluentValidationQueryHandler(IEnumerable<IValidator<TQuery>> validators)
    {
        _validators = validators.ToList();
    }

    public async Task<TResponse?> HandleAsync(TQuery request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken)
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
