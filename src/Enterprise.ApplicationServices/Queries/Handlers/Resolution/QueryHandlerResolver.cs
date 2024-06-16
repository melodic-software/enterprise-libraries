using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Resolution;
using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Core.Queries.Model.Alternate;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.ApplicationServices.Queries.Handlers.Resolution;

public class QueryHandlerResolver : IResolveQueryHandler
{
    private readonly IServiceProvider _serviceProvider;

    public QueryHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public IHandleQuery<TResult> GetQueryHandler<TResult>(IQuery query)
    {
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResult));
        return (IHandleQuery<TResult>)_serviceProvider.GetService(handlerType);
    }

    /// <inheritdoc />
    public IHandleQuery<TResult> GetQueryHandler<TResult>(IQuery<TResult> query)
    {
        Type queryType = query.GetType();
        Type handlerType = typeof(IHandleQuery<,>).MakeGenericType(queryType, typeof(TResult));
        return (IHandleQuery<TResult>)_serviceProvider.GetRequiredService(handlerType);
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(TQuery query)
        where TQuery : class, IQuery
    {
        return _serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResult>>();
    }

    /// <inheritdoc />
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(IQuery<TResult> query)
        where TQuery : class, IQuery<TResult>
    {
        return _serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResult>>();
    }
}
