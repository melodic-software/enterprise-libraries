using Enterprise.ApplicationServices.Core.Queries;
using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.QueryHandlers;

public class QueryHandlerResolver : IResolveQueryHandler
{
    private readonly IServiceProvider _serviceProvider;

    public QueryHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(TQuery query) where TQuery : IBaseQuery
    {
        IHandleQuery<TQuery, TResponse> queryHandler = _serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResponse>>();

        return queryHandler;
    }
}