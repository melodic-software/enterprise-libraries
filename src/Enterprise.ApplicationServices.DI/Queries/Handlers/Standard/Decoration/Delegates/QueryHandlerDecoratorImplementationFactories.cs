using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;

public static class QueryHandlerDecoratorImplementationFactories
{
    public static IEnumerable<QueryHandlerDecoratorImplementationFactory<TQuery, TResult>> GetDefault<TQuery, TResult>()
        where TQuery : class, IQuery
    {
        return
        [
            (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TQuery>> validators = provider.GetServices<IValidator<TQuery>>();
                IHandleQuery<TQuery, TResult> decorator = new FluentValidationQueryHandler<TQuery, TResult>(queryHandler, decoratorService, validators);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleQuery<TQuery, TResult> decorator = new NullQueryValidationQueryHandler<TQuery, TResult>(queryHandler, decoratorService);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingQueryHandler<TQuery, TResult>> logger = provider.GetRequiredService<ILogger<ErrorHandlingQueryHandler<TQuery, TResult>>>();
                IHandleQuery<TQuery, TResult> decorator = new ErrorHandlingQueryHandler<TQuery, TResult>(queryHandler, decoratorService, logger);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingQueryHandler<TQuery, TResult>> logger = provider.GetRequiredService<ILogger<LoggingQueryHandler<TQuery, TResult>>>();
                IHandleQuery<TQuery, TResult> decorator = new LoggingQueryHandler<TQuery, TResult>(queryHandler, decoratorService, logger);
                return decorator;
            }
        ];
    }
}
