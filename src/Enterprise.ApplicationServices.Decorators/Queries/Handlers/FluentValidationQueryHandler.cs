using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model.NonGeneric;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services;
using FluentValidation;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class FluentValidationQueryHandler<TQuery, TResult> : QueryHandlerDecoratorBase<TQuery, TResult>
    where TQuery : class, IQuery
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;

    public FluentValidationQueryHandler(IHandleQuery<TQuery, TResult> queryHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TQuery>> validators) : base(queryHandler, decoratorService)
    {
        _validators = validators.ToList();
    }

    public override async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        if (!_validators.Any())
        {
            return await Decorated.HandleAsync(query, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TQuery>(query);

        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);

        return await Decorated.HandleAsync(query, cancellationToken);
    }
}
