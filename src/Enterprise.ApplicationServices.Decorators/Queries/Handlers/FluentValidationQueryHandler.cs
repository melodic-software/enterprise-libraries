using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services;
using Enterprise.Patterns.ResultPattern.Model;
using FluentValidation;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class FluentValidationQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IQuery
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;

    public FluentValidationQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TQuery>> validators) : base(queryHandler, decoratorService)
    {
        _validators = validators.ToList();
    }

    public override async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            await Decorated.HandleAsync(query, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TQuery>(query);

        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);

        Result<TResponse> response = await Decorated.HandleAsync(query, cancellationToken);

        return response;
    }
}
