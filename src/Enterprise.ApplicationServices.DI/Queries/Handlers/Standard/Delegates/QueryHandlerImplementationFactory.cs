using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Delegates;

public delegate QueryHandlerBase<TQuery, TResponse>
    QueryHandlerImplementationFactory<TQuery, TResponse>(IServiceProvider provider)
    where TQuery : IBaseQuery;

public delegate IHandleQuery<TResponse>
    QueryHandlerImplementationFactory<TResponse>(IServiceProvider provider);
