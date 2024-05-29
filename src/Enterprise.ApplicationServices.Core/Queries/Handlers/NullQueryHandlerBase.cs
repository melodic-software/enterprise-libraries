using Enterprise.ApplicationServices.Core.Queries.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

public abstract class NullQueryHandlerBase
{
    protected readonly ILogger Logger;

    protected NullQueryHandlerBase(ILogger logger)
    {
        Logger = logger;
    }

    protected void LogWarning(IQuery query)
    {
        string handlerTypeName = GetType().Name;
        string queryTypeName = query.GetType().Name;

        Logger.LogWarning(
            "The {HandlerType} has been sent a \"{QueryType}\" query. " +
            "The query will not be handled. " +
            "Please verify query handler registrations.",
            handlerTypeName, queryTypeName
        );
    }
}
