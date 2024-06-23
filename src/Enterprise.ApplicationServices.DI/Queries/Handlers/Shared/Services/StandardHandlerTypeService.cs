using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.Shared.Services;

internal static class StandardHandlerTypeService
{
    public static Type GetStandardHandlerType<TQuery, TResult>() where TQuery : class, IQuery => 
        typeof(IHandleQuery<TQuery, TResult>);
}
