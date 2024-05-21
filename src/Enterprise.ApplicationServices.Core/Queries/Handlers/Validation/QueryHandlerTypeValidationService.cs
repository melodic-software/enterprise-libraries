using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Validation;

public static class QueryHandlerTypeValidationService
{
    public static void ValidateType<TQuery, TResponse>(TQuery query, IHandleQuery<TResponse> queryHandler)
        where TQuery : IBaseQuery
    {
        ValidateType(query, typeof(TQuery), queryHandler);
    }

    public static void ValidateType<TQuery, TResponse>(TQuery query, IHandleQuery<TQuery, TResponse> queryHandler)
        where TQuery : IBaseQuery
    {
        ValidateType(query, typeof(TQuery), queryHandler);
    }

    public static void ValidateType<TResponse>(IBaseQuery query, Type expectedQueryType, IHandleQuery<TResponse> queryHandler)
    {
        Type queryType = query.GetType();

        bool queryCanBeHandled = queryType.IsAssignableFrom(expectedQueryType);

        if (queryCanBeHandled)
        {
            return;
        }

        Type queryHandlerType = queryHandler.GetType();

        throw new InvalidOperationException(QueryCannotBeHandled(queryType, queryHandlerType));
    }

    private static string QueryCannotBeHandled(Type queryType, Type queryHandlerType) =>
        $"A query of type \"{queryType.FullName}\" cannot be handled by \"{queryHandlerType.FullName}\".";
}
