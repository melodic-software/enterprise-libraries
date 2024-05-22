using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.QueryHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.FluentValidation.Services;
using FluentValidation;

namespace Enterprise.ApplicationServices.Decorators.QueryHandlers;

public class FluentValidationQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    private readonly IReadOnlyCollection<IValidator<TQuery>> _validators;

    public FluentValidationQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService,
        IEnumerable<IValidator<TQuery>> validators) : base(queryHandler, decoratorService)
    {
        _validators = validators.ToList();
    }

    public override async Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            await Decorated.HandleAsync(query, cancellationToken);
        }

        IValidationContext validationContext = new ValidationContext<TQuery>(query);

        await FluentValidationService.ExecuteValidationAsync(_validators, validationContext);

        TResponse response = await Decorated.HandleAsync(query, cancellationToken);

        return response;
    }
}
