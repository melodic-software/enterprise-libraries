namespace Enterprise.ApplicationServices.Core.Queries.Handlers;

/// <summary>
/// Implementations perform the query execution and mapping logic (if required).
/// This is used to externalize the actual query implementation to an infrastructure layer.
/// This reduces the number of components that need to be created while retaining clean architecture.
/// When partnered with prebuilt base query handlers and DI factory methods, this becomes more streamlined.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryLogic<in TQuery, TResponse> where TQuery : IBaseQuery
{
    /// <summary>
    /// Execute the query logic.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> ExecuteAsync(TQuery query, CancellationToken cancellationToken);
}