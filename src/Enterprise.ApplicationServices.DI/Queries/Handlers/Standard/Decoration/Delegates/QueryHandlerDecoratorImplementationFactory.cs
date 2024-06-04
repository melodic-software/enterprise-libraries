using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Standard.Decoration.Delegates;

public delegate IHandleQuery<TQuery, TResponse>
    QueryHandlerDecoratorImplementationFactory<TQuery, TResponse>(IServiceProvider provider, IHandleQuery<TQuery, TResponse> queryHandler)
    where TQuery : IBaseQuery;
