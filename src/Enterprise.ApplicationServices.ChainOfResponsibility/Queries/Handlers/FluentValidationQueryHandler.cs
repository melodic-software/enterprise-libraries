using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;
using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;
using Enterprise.FluentValidation.Services.Generic;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.ChainOfResponsibility.Queries.Handlers;

public class FluentValidationQueryHandler<TQuery, TResult> : IHandler<TQuery, TResult>
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;
    private readonly ILogger<FluentValidationQueryHandler<TQuery, TResult>> _logger;

    public FluentValidationQueryHandler(
        IEnumerable<IValidator<TQuery>> validators,
        ILogger<FluentValidationQueryHandler<TQuery, TResult>> logger)
    {
        _logger = logger;
        _validators = validators.ToList();
    }

    public async Task<TResult?> HandleAsync(TQuery request, SuccessorDelegate<TResult> next, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        IValidationContext validationContext = new ValidationContext<TQuery>(request);

        _logger.LogDebug("Executing fluent validation.");
        TResult? result = await FluentValidationService.ExecuteValidationAsync<TResult>(_validators, validationContext);
        _logger.LogDebug("Fluent validation completed.");

        if (!Equals(result, default(TResult)))
        {
            return result;
        }

        return await next();
    }
}
