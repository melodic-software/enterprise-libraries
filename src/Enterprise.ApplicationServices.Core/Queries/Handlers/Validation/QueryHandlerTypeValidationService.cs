using Enterprise.ApplicationServices.Core.Queries.Handlers.Unbound;
using Enterprise.ApplicationServices.Core.Queries.Model;

namespace Enterprise.ApplicationServices.Core.Queries.Handlers.Validation;

public static class QueryHandlerTypeValidationService
{
    public static void ValidateType<TQuery, TResult>(TQuery query, IHandleQuery<TResult> queryHandler)
        where TQuery : class, IQuery
    {
        ValidateType(query, typeof(TQuery), queryHandler);
    }

    public static void ValidateType<TQuery, TResult>(TQuery query, IHandleQuery<TQuery, TResult> queryHandler)
        where TQuery : class, IQuery
    {
        ValidateType(query, typeof(TQuery), queryHandler);
    }

    public static void ValidateType<TResult>(IQuery query, Type expectedQueryType, IHandleQuery<TResult> queryHandler)
    {
        Type queryType = query.GetType();

        bool queryCanBeHandled = queryType.IsAssignableTo(expectedQueryType);

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
