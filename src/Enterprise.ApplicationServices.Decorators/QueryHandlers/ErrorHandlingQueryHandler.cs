using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Decorators.QueryHandlers.Abstract;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Decorators.QueryHandlers;

public class ErrorHandlingQueryHandler<TQuery, TResponse> : QueryHandlerDecoratorBase<TQuery, TResponse>
    where TQuery : IBaseQuery
{
    private readonly ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> _logger;

    public ErrorHandlingQueryHandler(IHandleQuery<TQuery, TResponse> queryHandler,
        IGetDecoratedInstance decoratorService,
        ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> logger) : base(queryHandler, decoratorService)
    {
        _logger = logger;
    }

    public override Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken)
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