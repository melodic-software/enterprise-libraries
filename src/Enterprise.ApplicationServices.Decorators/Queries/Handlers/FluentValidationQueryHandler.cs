using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.Base;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services.Generic;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class FluentValidationQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IBaseQuery
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;
    private readonly ILogger<FluentValidationQueryHandler<TQuery, TResult>> _logger;

    public FluentValidationQueryHandler(IHandleQuery<TQuery, TResult> queryHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TQuery>> validators,
        ILogger<FluentValidationQueryHandler<TQuery, TResult>> logger) : base(queryHandler, decoratorService)
    {
        _logger = logger;
        _validators = validators.ToList();
    }

    public override async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            return await Decorated.HandleAsync(query, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TQuery>(query);

        _logger.LogDebug("Executing fluent validation.");
        TResult? result = await FluentValidationService.ExecuteValidationAsync<TResult>(_validators, validationContext);
        _logger.LogDebug("Fluent validation completed.");

        if (!Equals(result, default(TResult)))
        {
            return result;
        }

        return await Decorated.HandleAsync(query, cancellationToken);
    }
}
