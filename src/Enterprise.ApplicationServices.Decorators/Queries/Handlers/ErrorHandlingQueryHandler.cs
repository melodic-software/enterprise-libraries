using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Patterns.ResultPattern.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.Queries.Handlers;

public class ErrorHandlingQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IQuery
{
    private readonly ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> _logger;

    public ErrorHandlingQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> logger) : base(queryHandler, decoratorService)
    {
        _logger = logger;
    }

    public override Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return Decorated.HandleAsync(query, cancellationToken);
        }
        catch (Exception exception)
        {
            Type queryType = typeof(TQuery);
            _logger.LogError(exception, "An error occurred while handling the \"{QueryType}\" query.", queryType.Name);
            throw;
        }
    }
}
